//this file was generated by RACC
//source grammar:..\..\..\..\Tests\RACC\test_cls\test_cls.yrd
//date:11/24/2010 12:34:09

module RACC.Actions

open Yard.Generators.RACCGenerator

let value x =
    ((x:>Lexeme<string>).value)

let s0 expr = 
    match expr with
    | RESeq [x0] -> 
        let (res) =
            let yardElemAction expr = 
                match expr with
                | REClosure(lst) -> 
                    let yardLAltAction expr = 
                        match expr with
                        | REAlt(Some(x), None) -> 
                            let yardLAltAction expr = 
                                match expr with
                                | RESeq [_] -> 

                                    box ()
                            yardLAltAction x 
                        | REAlt(None, Some(x)) -> 
                            let yardRAltAction expr = 
                                match expr with
                                | RESeq [_] -> 

                                    box ()
                            yardRAltAction x 

                    yardLAltAction expr

            yardElemAction(x0)
        box 1//(List.map value res)


let ruleToAction = dict [|("s",s0)|]

