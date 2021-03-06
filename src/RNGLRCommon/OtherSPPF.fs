﻿module Yard.Generators.RNGLR.OtherSPPF

open System.Collections.Generic
open Yard.Generators.Common.AST
open Yard.Generators.Common.AstNode
open Yard.Generators.Common.DataStructures
open FSharpx.Collections.Experimental

[<AllowNullLiteral>]
type OtherAST =
    val mutable first : OtherFamily
    val mutable other : OtherFamily[]
    val mutable pos : int
    val mutable parent : UsualOne<obj>
    new (f, o) = {pos = -1; first = f; other = o; parent = Unchecked.defaultof<UsualOne<obj>>}
    new (f) = new OtherAST(f, null)
    
    member inline this.findFamily f =
        if f this.first then Some this.first
        elif this.other <> null then Array.tryFind f this.other
        else None

    member this.addParent (p : obj) = 
        let newParent = 
            if this.parent.first = Unchecked.defaultof<obj> 
            then new UsualOne<_>(p, [||])
            else new UsualOne<_>(this.parent.first, Seq.append this.parent.other [|p|] |> Array.ofSeq)
        this.parent <- newParent

and OtherFamily =
    val mutable parent : obj
    val prod : int
    val mutable nodes : OtherNodes
    new (pro, n) = {prod = pro; nodes = n; parent = Unchecked.defaultof<UsualOne<obj>>}

    member this.addParent (p : obj) = this.parent <- p

    member this.ReplaceNodes (nodes) = this.nodes <- nodes

and OtherNodes =
    val mutable parent : UsualOne<obj>
    val mutable fst : obj
    val mutable snd : obj
    val mutable other : obj[]

    new (f, s, o) = {fst = f; snd = s; other = o; parent = Unchecked.defaultof<UsualOne<obj>>}
    new (f, s) = new OtherNodes(f, s, null)
    new (f) = new OtherNodes(f, null, null)

    new (arr : array<_>) =
        let mutable fs = null
        let mutable sn = null
        let mutable other = null
        if arr <> null then
            if arr.Length > 0 
            then
                fs <- arr.[0]
                if arr.Length > 1 
                then
                    sn <- arr.[1]
                    if arr.Length > 2 
                    then
                        other <- arr.[2..]
        {fst = fs; snd = sn; other = other; parent = Unchecked.defaultof<UsualOne<obj>>}
            
            
        member nodes.doForAll f =
            if nodes.fst <> null then
                f nodes.fst
                if nodes.snd <> null then
                    f nodes.snd
                    if nodes.other <> null then
                        for x in nodes.other do
                            f x

        member this.addParent (p : obj) = 
            this.doForAll 
                (
                    fun node -> 
                        match node with 
                        | :? OtherAST as ast -> ast.addParent p
                        | _ -> ()
                )
                
            let newParent = 
                if this.parent.first = Unchecked.defaultof<obj> 
                then new UsualOne<_>(p, [||])
                else new UsualOne<_>(this.parent.first, Seq.append this.parent.other [|p|] |> Array.ofSeq)
                
            this.parent <- newParent

            
        member nodes.exist f = 
            if nodes.fst <> null 
            then 
                if f nodes.fst then true
                else 
                    if nodes.snd <> null 
                    then 
                        if f nodes.snd then true
                        else
                            if nodes.other <> null 
                            then Array.exists (fun node -> f node) nodes.other
                            else false
                    else false
            else false
            
        /// <summary>
        /// applies function to nodes which are located to the right than node nd. 
        /// Right sibling of nd is first.
        /// </summary>
        member nodes.doForAllAfterNode nd f =
            let mutable needDo = false
            if nodes.fst <> null 
            then
                needDo <- nodes.fst = nd
                if nodes.snd <> null 
                then 
                    if needDo then f nodes.snd
                    needDo <- needDo || nodes.snd = nd
                    if nodes.other <> null 
                    then
                        for i = 0 to nodes.other.Length - 1 do
                            if needDo 
                            then 
                                f nodes.other.[i]
                            needDo <- needDo || nodes.other.[i] = nd

        /// <summary>
        /// applies function f to nodes which are located to the left than node nd. 
        /// Left sibling of nd is first.
        /// </summary>
        member nodes.doForAllBeforeNode nd f = 
            if nodes.fst <> null && nodes.fst <> nd
            then 
                if nodes.snd <> null && nodes.snd <> nd
                then 
                    if nodes.other <> null 
                    then 
                        let bound = 
                            let index = Array.tryFindIndex (fun n -> n = nd) nodes.other
                            if index.IsSome 
                            then index.Value - 1
                            else nodes.other.Length - 1
                        for i = bound downto 0 do
                            f nodes.other.[i]
                    f nodes.snd
                f nodes.fst
            
        /// <summary>
        /// Applies function to all nodes (right node is first)
        /// </summary>
        member nodes.doForAllRev f =
            if nodes.fst <> null then
                if nodes.snd <> null then
                    if nodes.other <> null then
                        for i = nodes.other.Length - 1 downto 0 do
                            f nodes.other.[i]
                    f nodes.snd
                f nodes.fst

                    
type private DotNodeType = Prod | AstNode

type private Context = 
    struct
        val parent : OtherFamily
        val child : obj
        val count : int

        new (p, ch, co) = {parent = p; child = ch; count = co}
    end

[<AllowNullLiteral>]
type OtherTree<'TokenType> (tree : Yard.Generators.Common.AST.Tree<'TokenType>) = 
    let tokens = tree.Tokens
    let root = 
        let root = 
            match tree.Root with
            | :? AST as ast -> ast
            | :? Epsilon -> Unchecked.defaultof<_>
            | _ -> failwith "Strange tree - singleNode with non-negative value"

        let dict = new System.Collections.Generic.Dictionary<AST, OtherAST>()
        
        // to avoid problems with cycles
        let knownNodes = new ResizeArray<_>()
        
        let nodesDict = System.Collections.Generic.Dictionary<Nodes, OtherNodes>()
        // reverse edges that will be added after all
        let postActions = new System.Collections.Generic.Dictionary<OtherFamily, Nodes>() 
        let rec processFamily (family : Family) = 
            let processAST (ast : AST) = 
                if dict.ContainsKey ast 
                then dict.[ast]
                else
                    let fstChild = processFamily ast.first
                    let otherChildren = 
                        if ast.other = null
                        then null
                        else Array.map (fun child -> processFamily child) ast.other

                    let newAST = new OtherAST (fstChild, otherChildren)
                    dict.Add (ast, newAST)

                    fstChild.addParent newAST
                    if otherChildren <> null
                    then otherChildren |> Array.iter (fun child -> child.addParent newAST) 
                    newAST
            
            let processNode (node : obj) (newNodes : list<_> ref)= 
                let newElem = 
                    match node with 
                    | :? AST as ast -> 
                        if dict.ContainsKey ast
                        then box <| dict.[ast]
                        else box <| processAST ast
                    | :? Terminal as term -> box term
                    | :? Epsilon as eps -> box eps
                    | _ -> failwithf "Unexpected node type in OtherSppf: %s" <| node.GetType().ToString()
                
                newNodes := newElem :: !newNodes
            
            if family.nodes.isForAll (fun node -> knownNodes.Contains node)
            then
                let fakeNodes = new OtherNodes(new obj())
                let newFamily = new OtherFamily (family.prod, fakeNodes)
                postActions.Add (newFamily, family.nodes)
                newFamily
            else
                let newNodes = ref []
                family.nodes.doForAll (fun node -> 
                    knownNodes.Add node
                    processNode node newNodes)
                let newNodes = new OtherNodes(!newNodes |> List.rev |> Array.ofList)
                nodesDict.Add (family.nodes, newNodes)
                let newFamily = new OtherFamily (family.prod, newNodes)
                newNodes.addParent newFamily
                newFamily

        let addReverseEdges() = 
            for pair in postActions do
                let otherFamily = pair.Key
                let oldNodes = pair.Value
                
                let newNodes = 
                    if nodesDict.ContainsKey oldNodes 
                    then 
                        nodesDict.[oldNodes]
                    else
                        let children = ref []
                        let handle (node : obj) = 
                            let newElem = 
                                match node with 
                                | :? AST as ast -> 
                                    if dict.ContainsKey ast
                                    then box <| dict.[ast]
                                    else failwith "Unexpected AST"
                                | :? Terminal as terminal -> box terminal.TokenNumber
                                | _ -> failwithf "Unexpected node type in OtherSppf: %s" <| node.GetType().ToString()
                
                            children := newElem :: !children
                
                        oldNodes.doForAll (fun node -> handle node)
                        new OtherNodes(!children |> List.rev |> Array.ofList)
                
                otherFamily.ReplaceNodes newNodes
                newNodes.addParent otherFamily

        let family = processFamily root.first
        
        addReverseEdges()
        let rootAST = new OtherAST(family)
        family.addParent rootAST
        rootAST

    let order =
        let stack = new Stack<_>()
        stack.Push root
        let res = new BlockResizeArray<_>()
        //if not isEpsilon then
        while stack.Count > 0 do
            let u = stack.Pop()
            let children = u
            if children.pos = -2 
            then
                children.pos <- res.Length
                res.Add u
            elif children.pos = -1 
            then
                children.pos <- -2
                stack.Push u
                let inline handle (family : OtherFamily) = 
                    let inline handleAst (ast : obj) =
                        match ast with
                        | :? OtherAST as ast ->
                            if ast.pos = -1 then
                                stack.Push ast
                        | _ -> ()
                    family.nodes.doForAllRev handleAst
                handle children.first
                if children.other <> null then
                    for family in children.other do
                        handle family
        res.ToArray()

    let familyToTokens = 
        let dict = new Dictionary<OtherFamily, int list>()
        // to avoid problems with cycles
        let processedAst = new ResizeArray<OtherAST>()

        let rec calcTokens (family : OtherFamily) = 
            if dict.ContainsKey family 
            then 
                dict.[family]
            else
                let tokens = ref []
                family.nodes.doForAll (fun node -> 
                    match node with
                    | :? OtherAST as ast -> 
                        if not <| processedAst.Contains ast
                        then
                            processedAst.Add ast
                            let temp = ref <| calcTokens ast.first
                            if ast.other <> null
                            then 
                                for fam in ast.other do
                                    temp := !temp @ calcTokens fam
                            tokens := !tokens @ !temp
                            processedAst.Remove ast |> ignore
                    | :? Terminal as t ->
                        tokens := t.TokenNumber :: !tokens
                    | _ -> ()
                )
                
                dict.Add (family, !tokens)
                
                !tokens

        calcTokens root.first |> ignore
        dict

    let findNodeWithParents (now : 'range) (tokenToPos : 'TokenType -> seq<'range>)= 
        let parents = ref []
        let child = ref null

        let containsRange number = 
            tokenToPos tokens.[number]
            |> Seq.exists (fun range -> range = now)

        let rec handleFamily (family : OtherFamily) = 
            family.nodes.doForAll (fun node -> 
                match node with
                | :? OtherAST as ast -> 
                    
                    let famTokens = familyToTokens.[ast.first]

                    let interesting = familyToTokens.Values
                    interesting.ToString() |> ignore
                    
                    if List.exists (fun t -> containsRange t) famTokens
                    then handleFamily ast.first
                    
                    if ast.other <> null 
                    then
                        ast.other
                        |> Array.iter (fun fam ->
                            if List.exists (fun t -> containsRange t) familyToTokens.[fam]
                            then handleFamily fam
                        )
                
                | :? Terminal as t -> 
                    let isNewParent = not <| List.exists (fun fam -> fam = family) !parents
                    if containsRange t.TokenNumber && isNewParent
                    then
                        parents := family :: !parents
                        child := node
                | :? Epsilon -> ()
                | _ -> failwithf "Unexpected node type in OtherSppf: %s" <| child.GetType().ToString()
                )
        handleFamily root.first
        !child, !parents

    /// <summary>
    /// Returnes all paired tokens for token that is located in range. 
    /// For example it returns all paired right_brackets for left_bracket 
    /// </summary>
    /// <param name="left">Number of left paired token</param>
    /// <param name="right">Number of right paired token</param>
    /// <param name="toRight">True, if we search close token.</param>
    member this.FindAllPair (left : int) (right : int) (now : 'range) toRight tokenToNumber (tokenToPos : 'TokenType -> seq<'range>)= 
        let leaf, parents = findNodeWithParents now tokenToPos
        let res = new ResizeArray<_>()
        let contexts = new Stack<_>()

        parents
        |> List.iter (fun family -> contexts.Push <| new Context (family, leaf, 1))

        let nowNumber, pairNumber = if toRight then left, right else right, left
        let handleSomeNodes node (family : OtherFamily) f = 
            if toRight 
            then family.nodes.doForAllAfterNode node f
            else family.nodes.doForAllBeforeNode node f

        let handleAllNodes (family : OtherFamily) f = 
            if toRight
            then family.nodes.doForAll f
            else family.nodes.doForAllRev f

        while contexts.Count > 0 do
            let state = contexts.Pop()
            let mutable parent = box state.parent
            let child = ref state.child
            let count = ref state.count

            let rec processFamily (family : OtherFamily) = 
                //familyToTokens.ContainsKey family is always true
                if familyToTokens.[family] 
                    |> List.exists (fun t -> 
                        let tokNumber = tokenToNumber tokens.[t]
                        tokNumber = pairNumber || tokNumber = nowNumber) 
                then 
                    let handle (node : obj) =
                        match node with 
                        | :? Terminal as t -> 
                            let tokNumber = tokenToNumber tokens.[t.TokenNumber]
                            if nowNumber = tokNumber then incr count
                            elif pairNumber = tokNumber then decr count
            
                            if !count = 0 then res.Add tokens.[t.TokenNumber]
                        | :? OtherAST as ast -> processAST ast
                        | :? Epsilon -> ()
                        | _ -> failwithf "Unexpected node type in OtherSppf: %s" <| node.GetType().ToString()
        
                    if family.nodes.exist (fun node -> node = !child)
                    then handleSomeNodes !child family handle 
                    else handleAllNodes family (fun node -> handle node)

            and processAST (ast : OtherAST) = 
                let value = !count
                let oldChild = !child
                processFamily ast.first
                
                if ast.other <> null
                then 
                    ast.other
                    |> Array.iter 
                        (
                            fun fam -> 
                                count := value
                                child := oldChild
                                processFamily fam
                        )

            match parent with
            | :? OtherFamily as fam -> processFamily fam
            | :? OtherAST as ast -> processAST ast
            | _ -> failwith ""

            let isEnd = ref false
            while not !isEnd && !count <> 0 do
                let father = 
                    match parent with
                    | :? OtherAST as ast -> 
                        child := box parent
                        if ast.parent.other <> null && ast.parent.other.Length > 0 
                        then
                            ast.parent.other
                            |> Array.iter 
                                (
                                    fun parent -> 
                                        let newContext = new Context (parent :?> OtherFamily, !child, !count)
                                        contexts.Push newContext
                                )
                        ast.parent.first

                    | :? OtherFamily as fam -> fam.parent
                    | _ -> failwithf "Unexpected error."
                
                match father with
                | :? OtherAST as ast -> processAST ast
                | :? OtherFamily as fam -> processFamily fam
                | _ -> isEnd := true
                
                parent <- father
        res

    /// <summary>
    /// Prints ast in console
    /// </summary>
    member this.PrintAst() =            
        let processed = new ResizeArray<_>()
        let rec printAst ind ast =
            let printInd num (x : 'a) =
                printf "%s" (String.replicate (num * 4) " ")
                printfn x

            match (ast : obj) with
            | :? Epsilon -> printInd ind "e"
            | :? Terminal as t -> printInd ind "t: %A" tokens.[t.TokenNumber]
            | :? OtherAST as fam ->
                
                processed.Add(ast)
                let children = fam
                let needGroup = children.other <> null
                if needGroup 
                then printInd ind "^^^^"
                
                let inline handle separate (family : OtherFamily) =
                    if separate 
                    then printInd ind "----"
                    printInd ind "prod %d" family.prod
                    family.nodes.doForAll (printAst <| ind + 1)
                
                if not <| processed.Contains ast
                then
                    children.first |> handle false
                
                    if needGroup 
                    then children.other |> Array.iter (handle true)
                       
                    if needGroup then printInd ind "vvvv"
            | _ -> failwithf "Unexpected node type in OtherSppf: " <| ast.GetType().ToString()
        printAst 0 root

    /// <summary>
    /// Prints sppf in .dot file
    /// </summary>
    member this.AstToDot (indToString : int -> string) tokenToNumber (leftSide : array<int>) (path : string) =
        let next =
            let cur = ref order.Length
            fun () ->
                incr cur
                !cur

        use out = new System.IO.StreamWriter (path : string)
        out.WriteLine("digraph AST {")
        
        let createNode num isAmbiguous isTerminal nodeType (str : string) =
            let label =
                let cur = str.Replace("\n", "\\n").Replace ("\r", "")
                if not isAmbiguous 
                then cur
                else cur + " !"
            let shape =
                match nodeType with
                | AstNode -> ",shape=box"
                | Prod -> ""
            let color =
                if isTerminal then ",style=\"filled\",fillcolor=gray"
                elif not isAmbiguous then ""
                else ",style=\"filled\",fillcolor=red"
            out.WriteLine ("    " + num.ToString() + " [label=\"" + label + "\"" + color + shape + "]")
        
        let createEdge (b : int) (e : int) isBold (str : string) =
            let label = str.Replace("\n", "\\n").Replace ("\r", "")
            let bold = 
                if not isBold 
                then ""
                else "style=bold,width=10,"
            out.WriteLine ("    " + b.ToString() + " -> " + e.ToString() + " [" + bold + "label=\"" + label + "\"" + "]")
        
        let createEpsilon ind = 
            let res = next()
            createNode res false false AstNode ("n " + indToString (-1-ind))
            let u = next()
            createNode u false false AstNode "eps"
            createEdge res u true ""
            res
        let createTerm t =
            let res = next()
            createNode res false true AstNode ("t " + indToString (tokenToNumber tokens.[t]))
            res
        (*if not isEpsilon then*)
        for i = order.Length-1 downto 0 do
            let x = order.[i]
            if x.pos <> -1 
            then
                let children = x
                    
                let label = 
                    if children.first.prod < leftSide.Length 
                    then indToString leftSide.[children.first.prod]
                    else "error"
                     
                createNode i (children.other <> null) false AstNode ("n " + label)
                     
                let inline handle (family : OtherFamily) =
                    let u = next()
                    createNode u false false Prod ("prod " + family.prod.ToString())
                    createEdge i u true ""
                    family.nodes.doForAll <| fun child ->
                        let v = 
                            match child with
                            | :? OtherAST as v -> v.pos
                            | :? Epsilon as eps -> createEpsilon eps.EpsilonNonTerm
                            | :? Terminal as t -> createTerm t.TokenNumber
                            | _ -> failwithf "Unexpected node type in OtherSppf: %s" <| child.GetType().ToString()
                        createEdge u v false ""
                children.first |> handle
                if children.other <> null 
                then children.other |> Array.iter handle
//        else createEpsilon (getSingleNode root) |> ignore
        
        out.WriteLine("}")
        out.Close()