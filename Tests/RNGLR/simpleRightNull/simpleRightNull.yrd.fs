module RNGLR.Parse
open Yard.Generators.RNGLR.Parser
open Yard.Generators.RNGLR
type Token =
        | A of int
        | EOF of int

let buildAst<'a> =
    let inline unpack x = x >>> 16, x <<< 16 >>> 16
    let small_gotos =
        [|0, [|1,1; 4,2|]; 2, [|1,3; 4,2|]; 3, [|2,4|]|]
    let gotos = Array.zeroCreate 5
    for i = 0 to 4 do
        gotos.[i] <- Array.create 6 None
    for (i,t) in small_gotos do
        for (j,x) in t do
            gotos.[i].[j] <- Some  x
    let lists_reduces = [|[]; [1,1]; [1,2]; [1,3]|]
    let small_reduces =
        [|131073; 327681; 196609; 327682; 262145; 327683|]
    let reduces = Array.zeroCreate 5
    for i = 0 to 4 do
        reduces.[i] <- Array.create 6 []
    let init_reduces =
        let mutable cur = 0
        while cur < small_reduces.Length do
            let i,length = unpack small_reduces.[cur]
            cur <- cur + 1
            for k = 0 to length-1 do
                let j,x = unpack small_reduces.[cur + k]
                reduces.[i].[j] <-  lists_reduces.[x]
            cur <- cur + length
    let lists_zeroReduces = [|[]; [2; 0]; [0]; [3]|]
    let small_zeroReduces =
        [|1; 327681; 131073; 327682; 196609; 327683|]
    let zeroReduces = Array.zeroCreate 5
    for i = 0 to 4 do
        zeroReduces.[i] <- Array.create 6 []
    let init_zeroReduces =
        let mutable cur = 0
        while cur < small_zeroReduces.Length do
            let i,length = unpack small_zeroReduces.[cur]
            cur <- cur + 1
            for k = 0 to length-1 do
                let j,x = unpack small_zeroReduces.[cur + k]
                zeroReduces.[i].[j] <-  lists_zeroReduces.[x]
            cur <- cur + length
    let small_acc = [1; 0]
    let accStates = Array.zeroCreate 5
    for i = 0 to 4 do
        accStates.[i] <- List.exists ((=) i) small_acc
    let rules = [|4; 1; 2; 1|]
    let rulesStart = [|0; 0; 3; 4|]
    let leftSide =
        [|1; 1; 3; 2|]
    let startRule = 2
    let eofIndex = 5
    let tokenToNumber = function
        | A _ -> 4
        | EOF _ -> 5
    let parserSource = new ParserSource<_> (gotos, reduces, zeroReduces, accStates, rules, rulesStart, leftSide, startRule, eofIndex, tokenToNumber)
    buildAst<_> parserSource

