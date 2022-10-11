﻿using Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Data.Board
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BoardProvider : MonoProvider<BoardComponent>
    {
        
    }
}