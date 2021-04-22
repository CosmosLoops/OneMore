﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace Cosmos.Security.Verification
{
    public class FnvConfig
    {
        public int HashSizeInBits { get; internal set; }

        public BigInteger Prime { get; set; }

        public BigInteger Offset { get; set; }

        public FnvConfig Clone() => new()
        {
            HashSizeInBits = HashSizeInBits,
            Prime = Prime,
            Offset = Offset
        };

        private static readonly IReadOnlyDictionary<int, FnvConfig> _predefinedConfigs = new Dictionary<int, FnvConfig>()
        {
            {
                32,
                new FnvConfig()
                {
                    HashSizeInBits = 32,
                    Prime = new BigInteger(16777619),
                    Offset = new BigInteger(2166136261)
                }
            },
            {
                64,
                new FnvConfig()
                {
                    HashSizeInBits = 64,
                    Prime = new BigInteger(1099511628211),
                    Offset = new BigInteger(14695981039346656037)
                }
            },
            {
                128,
                new FnvConfig()
                {
                    HashSizeInBits = 128,
                    Prime = BigInteger.Parse("309485009821345068724781371"),
                    Offset = BigInteger.Parse("144066263297769815596495629667062367629")
                }
            },
            {
                256,
                new FnvConfig()
                {
                    HashSizeInBits = 256,
                    Prime = BigInteger.Parse("374144419156711147060143317175368453031918731002211"),
                    Offset = BigInteger.Parse("100029257958052580907070968620625704837092796014241193945225284501741471925557")
                }
            },
            {
                512,
                new FnvConfig()
                {
                    HashSizeInBits = 512,
                    Prime = BigInteger.Parse("35835915874844867368919076489095108449946327955754392558399825615420669938882575126094039892345713852759"),
                    Offset = BigInteger.Parse("9659303129496669498009435400716310466090418745672637896108374329434462657994582932197716438449813051892206539805784495328239340083876191928701583869517785")
                }
            },
            {
                1024,
                new FnvConfig()
                {
                    HashSizeInBits = 1024,
                    Prime = BigInteger.Parse(
                        "5016456510113118655434598811035278955030765345404790744303017523831112055108147451509157692220295382716162651878526895249385292291816524375083746691371804094271873160484737966720260389217684476157468082573"),
                    Offset = BigInteger.Parse(
                        "14197795064947621068722070641403218320880622795441933960878474914617582723252296732303717722150864096521202355549365628174669108571814760471015076148029755969804077320157692458563003215304957150157403644460363550505412711285966361610267868082893823963790439336411086884584107735010676915")
                }
            }
        };


        /// <summary>
        /// Gets one of the predefined configurations as defined at http://www.isthe.com/chongo/tech/comp/fnv/index.html.
        /// </summary>
        /// <param name="hashSizeInBits">The desired hash length, in bits.</param>
        /// <returns>The predefined configuration instance.</returns>
        public static FnvConfig GetPredefinedConfig(int hashSizeInBits)
        {
            if (!_predefinedConfigs.TryGetValue(hashSizeInBits, out var config))
                throw new ArgumentOutOfRangeException(nameof(hashSizeInBits), hashSizeInBits, $"{nameof(hashSizeInBits)} must be a positive power of 2 between 32 and 1024.");
            return config;
        }
    }
}