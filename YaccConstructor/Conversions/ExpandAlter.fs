﻿//  Copyright 2009 by Ilia Chemodanov
//  Copyright 2012 by Semen Grigorev <rsdpiduy@gmail.com>
//
//  This file is part of YaccConctructor.
//
//  YaccConstructor is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

module Yard.Core.Conversions.ExpandTopLevelAlt

open Yard.Core
open Yard.Core.IL
open Yard.Core.IL.Production
open Yard.Core.IL.Production

open System

let extractOneRule (rule:Rule.t<'a,'b>) = 
    let rec expand = function
    | PAlt (a,b) -> {rule with body = a} :: expand b
    | a   -> [{rule with body = a}]    
    expand rule.body

type ExpandTopLevelAlt() = 
    inherit Conversion()
        override this.Name = "ExpandTopLevelAlt"
        override this.ConvertList (ruleList,_) = List.collect extractOneRule ruleList
        override this.EliminatedProductionTypes = ["PAlt"]