﻿let run inStr =
    let  buf = Lexing.LexBuffer<_>.FromString inStr 
    let tokens =
        seq { 
            while not buf.IsPastEndOfStream do
            let t = Lexer.token buf
            yield t
        }
        |> Yard.Generators.CYK.CodeTokenStream
        
    let cyk = new Yard.Generators.CYKGenerator.CYKCore()
    cyk.Recognize (Yard.Generators.CYK.rules, Yard.Generators.CYK.StartNTerm) tokens (fun x y z -> 0uy)

let time () =    
    let start = System.DateTime.Now
    let res = (run("2" + String.replicate 3000 "+")
//        run("2+3+5*6+9^9*4**7+3+8+2+3+5*6+9^9*4**7+3+8+3+8+2+3+5*6+9^9*4**7+3+8+3+8+2+3+5*6+9^9*4**7+3+8+3+8+2+3+5*6+9^9*4**7+3+8+3+5*6+9^9*4**7+3+8+2+3+5*6+9^9*4**7+3+8+3+8+2+3+5*6+9^9*4**7+3+8+3+8+2+3+5*6+9^9*4**7+3+8+3+8+2+3+5*6+9^9*4**7+3+8" 
//        //"9+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2"
//         + "+9+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2"
//         + "+9+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2"
//         + "+9+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2"
//         + "+9+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2"
//         + "+9+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2"
//         + "+9+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2"
//         + "+9+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2+2+8+9+2*2"
         )
    printfn "%s" (string res)
    printfn "%A" (System.DateTime.Now - start)
    //System.Console.ReadKey() |> ignore
do 
    time ()