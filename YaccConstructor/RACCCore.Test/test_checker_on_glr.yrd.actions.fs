//this file was generated by RACC
//source grammar:..\Tests\RACC\test_checker_on_glr\\test_checker_on_glr.yrd
//date:12/16/2010 17:05:27

module RACC.Actions_Checker_on_glr

open Yard.Generators.RACCGenerator

let value x = (x:>Lexeme<string>).value

type OpPrior =
   | T
   | B

let checker curOp lNextOp rNextOp =
    match curOp, lNextOp, rNextOp with 
    | B,_,T 
    | T,T,T -> true 
    | _,_,_ -> false

let s0 expr = 
    let inner  = 
        match expr with
        | RESeq [x0] -> 
            let (res:int,nextOp:OpPrior) =
                let yardElemAction expr = 
                    match expr with
                    | RELeaf e -> (e :?> _ ) 
                    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                yardElemAction(x0)
            (res)
        | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith
    box (inner)
let e1 expr = 
    let inner  = 
        match expr with
        | REAlt(Some(x), None) -> 
            let yardLAltAction expr = 
                match expr with
                | RESeq [x0] -> 
                    let (n) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf tNUMBER -> tNUMBER :?> 'a
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x0)
                    (value n |> int, T)
                | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

            yardLAltAction x 
        | REAlt(None, Some(x)) -> 
            let yardRAltAction expr = 
                match expr with
                | RESeq [x0; x1; x2] -> 
                    let (l,lNextOp) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf e -> (e :?> _ ) 
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x0)
                    let (op,curOp) =
                        let yardElemAction expr = 
                            match expr with
                            | REAlt(Some(x), None) -> 
                                let yardLAltAction expr = 
                                    match expr with
                                    | RESeq [_] -> 

                                        ( (+),B )
                                    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

                                yardLAltAction x 
                            | REAlt(None, Some(x)) -> 
                                let yardRAltAction expr = 
                                    match expr with
                                    | REAlt(Some(x), None) -> 
                                        let yardLAltAction expr = 
                                            match expr with
                                            | RESeq [_] -> 

                                                ( ( * ),T )
                                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

                                        yardLAltAction x 
                                    | REAlt(None, Some(x)) -> 
                                        let yardRAltAction expr = 
                                            match expr with
                                            | RESeq [_] -> 

                                                ( (-),B )
                                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

                                        yardRAltAction x 
                                    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nREAlt was expected." |> failwith

                                yardRAltAction x 
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nREAlt was expected." |> failwith

                        yardElemAction(x1)
                    let (r,rNextOp) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf e -> (e :?> _ ) 
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x2)
                    if not (checker curOp lNextOp rNextOp) then raise Constants.CheckerFalse

                    ((op l r),curOp)
                | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

            yardRAltAction x 
        | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nREAlt was expected." |> failwith
    box (inner)

let ruleToAction = dict [|("e",e1); ("s",s0)|]
