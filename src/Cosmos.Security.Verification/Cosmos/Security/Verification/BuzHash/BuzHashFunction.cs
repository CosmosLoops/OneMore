﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cosmos.Security.Verification.Core;

// ReSharper disable once CheckNamespace
namespace Cosmos.Security.Verification
{
    internal class BuzHashFunction : StreamableHashFunctionBase, IBuzHash
    {
        private readonly BuzHashConfig _config;
        private static readonly IEnumerable<int> _validHashSizes = new HashSet<int>() {8, 16, 32, 64};

        public BuzHashFunction(BuzHashConfig config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));
            _config = config.Clone();

            if (_config.Rtab == null || _config.Rtab.Count != 256)
                throw new ArgumentException($"{nameof(config.Rtab)} must be non-null list of 256 {nameof(UInt64)} values.", $"{nameof(config)}.{nameof(config.Rtab)}");

            if (!_validHashSizes.Contains(_config.HashSizeInBits))
                throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be contained within BuzHashBase.ValidHashSizes.");

            if (_config.ShiftDirection != CircularShiftDirection.Left && _config.ShiftDirection != CircularShiftDirection.Right)
                throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.ShiftDirection)}", _config.ShiftDirection, $"{nameof(config)}.{nameof(config.ShiftDirection)} must be a valid {nameof(CircularShiftDirection)} value.");
        }

        public BuzHashConfig Config => _config.Clone();

        public override int HashSizeInBits => _config.HashSizeInBits;

        public override IBlockTransformer CreateBlockTransformer()
        {
            return _config.HashSizeInBits switch
            {
                8 => new BlockTransformer_8Bit(_config),
                16 => new BlockTransformer_16Bit(_config),
                32 => new BlockTransformer_32Bit(_config),
                64 => new BlockTransformer_64Bit(_config),
                _ => throw new NotImplementedException()
            };
        }

        #region Internal Implementation of BlockTransformer

        private class BlockTransformer_8Bit : BlockTransformerBase<BlockTransformer_8Bit>
        {
            private IReadOnlyList<UInt64> _rtab;
            private CircularShiftDirection _shiftDirection;

            private byte _hashValue;

            public BlockTransformer_8Bit() { }

            public BlockTransformer_8Bit(BuzHashConfig config) : this()
            {
                _rtab = config.Rtab;
                _shiftDirection = config.ShiftDirection;

                _hashValue = (byte) config.Seed;
            }

            protected override void CopyStateTo(BlockTransformer_8Bit other)
            {
                base.CopyStateTo(other);

                other._hashValue = _hashValue;
                other._rtab = _rtab;
            }

            protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
            {
                var dataArray = data.Array;
                var dataCount = data.Count;
                var endOffset = data.Offset + dataCount;

                var tempHashValue = _hashValue;
                var tempRtab = _rtab;

                for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
                    tempHashValue = (byte) (CShift(tempHashValue, 1, _shiftDirection) ^ (byte) tempRtab[dataArray[currentOffset]]);

                _hashValue = tempHashValue;
            }

            protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
            {
                return new HashValue(new[] {_hashValue}, 8);
            }
        }

        private class BlockTransformer_16Bit : BlockTransformerBase<BlockTransformer_16Bit>
        {
            private IReadOnlyList<UInt64> _rtab;
            private CircularShiftDirection _shiftDirection;

            private UInt16 _hashValue;

            public BlockTransformer_16Bit() { }

            public BlockTransformer_16Bit(BuzHashConfig config) : this()
            {
                _rtab = config.Rtab;
                _shiftDirection = config.ShiftDirection;

                _hashValue = (UInt16) config.Seed;
            }

            protected override void CopyStateTo(BlockTransformer_16Bit other)
            {
                base.CopyStateTo(other);

                other._hashValue = _hashValue;
                other._rtab = _rtab;
            }

            protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
            {
                var dataArray = data.Array;
                var dataCount = data.Count;
                var endOffset = data.Offset + dataCount;

                var tempHashValue = _hashValue;
                var tempRtab = _rtab;

                for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
                    tempHashValue = (UInt16) (CShift(tempHashValue, 1, _shiftDirection) ^ (UInt16) tempRtab[dataArray[currentOffset]]);

                _hashValue = tempHashValue;
            }

            protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
            {
                return new HashValue(BitConverter.GetBytes(_hashValue), 16);
            }
        }

        private class BlockTransformer_32Bit : BlockTransformerBase<BlockTransformer_32Bit>
        {
            private IReadOnlyList<UInt64> _rtab;
            private CircularShiftDirection _shiftDirection;

            private UInt32 _hashValue;

            public BlockTransformer_32Bit() { }

            public BlockTransformer_32Bit(BuzHashConfig config) : this()
            {
                _rtab = config.Rtab;
                _shiftDirection = config.ShiftDirection;

                _hashValue = (uint) config.Seed;
            }

            protected override void CopyStateTo(BlockTransformer_32Bit other)
            {
                base.CopyStateTo(other);

                other._hashValue = _hashValue;
                other._rtab = _rtab;
            }

            protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
            {
                var dataArray = data.Array;
                var dataCount = data.Count;
                var endOffset = data.Offset + dataCount;

                var tempHashValue = _hashValue;
                var tempRtab = _rtab;

                for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
                    tempHashValue = (UInt32) (CShift(tempHashValue, 1, _shiftDirection) ^ (UInt32) tempRtab[dataArray[currentOffset]]);

                _hashValue = tempHashValue;
            }

            protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
            {
                return new HashValue(BitConverter.GetBytes(_hashValue), 32);
            }
        }

        private class BlockTransformer_64Bit : BlockTransformerBase<BlockTransformer_64Bit>
        {
            private IReadOnlyList<UInt64> _rtab;
            private CircularShiftDirection _shiftDirection;

            private UInt64 _hashValue;

            public BlockTransformer_64Bit() { }

            public BlockTransformer_64Bit(BuzHashConfig config) : this()
            {
                _rtab = config.Rtab;
                _shiftDirection = config.ShiftDirection;

                _hashValue = (UInt64) config.Seed;
            }

            protected override void CopyStateTo(BlockTransformer_64Bit other)
            {
                base.CopyStateTo(other);

                other._hashValue = _hashValue;
                other._rtab = _rtab;
            }

            protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
            {
                var dataArray = data.Array;
                var dataCount = data.Count;
                var endOffset = data.Offset + dataCount;

                var tempHashValue = _hashValue;
                var tempRtab = _rtab;

                for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
                    tempHashValue = (UInt64) (CShift(tempHashValue, 1, _shiftDirection) ^ (UInt64) tempRtab[dataArray[currentOffset]]);

                _hashValue = tempHashValue;
            }

            protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
            {
                return new HashValue(BitConverter.GetBytes(_hashValue), 64);
            }
        }

        #region Internal - CShift

        private static byte CShift(byte n, int shiftCount, CircularShiftDirection shiftDirection)
        {
            if (shiftDirection == CircularShiftDirection.Right)
                return RotateRight(n, shiftCount);

            return RotateLeft(n, shiftCount);
        }

        private static UInt16 CShift(UInt16 n, int shiftCount, CircularShiftDirection shiftDirection)
        {
            if (shiftDirection == CircularShiftDirection.Right)
                return RotateRight(n, shiftCount);

            return RotateLeft(n, shiftCount);
        }

        private static UInt32 CShift(UInt32 n, int shiftCount, CircularShiftDirection shiftDirection)
        {
            if (shiftDirection == CircularShiftDirection.Right)
                return RotateRight(n, shiftCount);

            return RotateLeft(n, shiftCount);
        }

        private static UInt64 CShift(UInt64 n, int shiftCount, CircularShiftDirection shiftDirection)
        {
            if (shiftDirection == CircularShiftDirection.Right)
                return RotateRight(n, shiftCount);

            return RotateLeft(n, shiftCount);
        }

        #endregion

        #region Internal - RotateLeft

        private static byte RotateLeft(byte operand, int shiftCount)
        {
            shiftCount &= 0x07;

            return (byte) (
                (operand << shiftCount) |
                (operand >> (8 - shiftCount)));
        }

        private static UInt16 RotateLeft(UInt16 operand, int shiftCount)
        {
            shiftCount &= 0x0f;

            return (UInt16) (
                (operand << shiftCount) |
                (operand >> (16 - shiftCount)));
        }

        private static UInt32 RotateLeft(UInt32 operand, int shiftCount)
        {
            shiftCount &= 0x1f;

            return
                (operand << shiftCount) |
                (operand >> (32 - shiftCount));
        }

        private static UInt64 RotateLeft(UInt64 operand, int shiftCount)
        {
            shiftCount &= 0x3f;

            return
                (operand << shiftCount) |
                (operand >> (64 - shiftCount));
        }

        #endregion

        #region Internal - RotateRight

        private static byte RotateRight(byte operand, int shiftCount)
        {
            shiftCount &= 0x07;

            return (byte) (
                (operand >> shiftCount) |
                (operand << (8 - shiftCount)));
        }

        private static UInt16 RotateRight(UInt16 operand, int shiftCount)
        {
            shiftCount &= 0x0f;

            return (UInt16) (
                (operand >> shiftCount) |
                (operand << (16 - shiftCount)));
        }

        private static UInt32 RotateRight(UInt32 operand, int shiftCount)
        {
            shiftCount &= 0x1f;

            return
                (operand >> shiftCount) |
                (operand << (32 - shiftCount));
        }

        private static UInt64 RotateRight(UInt64 operand, int shiftCount)
        {
            shiftCount &= 0x3f;

            return
                (operand >> shiftCount) |
                (operand << (64 - shiftCount));
        }

        #endregion

        #endregion
    }
}