//this tyables was generated by RACC
//source grammar:..\..\..\..\Tests\RACC\test_cls\test_cls.yrd
//date:11/24/2010 12:34:09

#light "off"
module Yard.Generators.RACCGenerator.Tables

open Yard.Generators.RACCGenerator

let autumataDict = 
dict [|("s",{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,(State 2));(3,DummyState);(4,DummyState);(5,DummyState)|];
   DStartState   = 2;
   DFinaleStates = Set.ofArray [|0;1;2|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol "MINUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5))|]|];
   ToStateID   = 0;
}
;{ 
   FromStateID = 0;
   Symbol      = (DSymbol "PLUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5))|]|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 0;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2));(FATrace (TAlt1E 5))|]|];
   ToStateID   = 3;
}
;{ 
   FromStateID = 1;
   Symbol      = (DSymbol "MINUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6))|]|];
   ToStateID   = 0;
}
;{ 
   FromStateID = 1;
   Symbol      = (DSymbol "PLUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6))|]|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt2E 6))|]|];
   ToStateID   = 4;
}
;{ 
   FromStateID = 2;
   Symbol      = (DSymbol "MINUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0))|]|];
   ToStateID   = 0;
}
;{ 
   FromStateID = 2;
   Symbol      = (DSymbol "PLUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0))|]|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 2;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TAlt1S 5));(FATrace (TSeqS 2));(FATrace (TSmbS 1))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TAlt2S 6));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0));(FATrace (TClsE 0));(FATrace (TSeqE 7))|];List.ofArray [|(FATrace (TSeqS 7));(FATrace (TClsS 0))|]|];
   ToStateID   = 5;
}
|];
}
)|]

let items = 
List.ofArray [|("s",0);("s",1);("s",2);("s",3);("s",4);("s",5)|]

let gotoSet = 
Set.ofArray [|(-1144263691,("s",0));(-1144264106,("s",0));(-1144264172,("s",0));(1800920813,("s",4));(1800920844,("s",3));(1800920910,("s",5));(452886726,("s",1));(452886823,("s",1));(452886885,("s",1))|]

