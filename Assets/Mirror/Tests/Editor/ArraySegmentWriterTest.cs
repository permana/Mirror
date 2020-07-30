using System;
using NUnit.Framework;

namespace Mirror.Tests
{
    [TestFixture]
    public class ArraySegmentWriterTest
    {
        #region ArraySegment<byte>

        // ArraySegment<byte> is a special case,  optimized for no copy and no allocation
        // other types are generated by the weaver

        struct ByteArraySegmentMessage : IMessageBase
        {
            public ArraySegment<byte> array;

            // Weaver will generate serialization
            public void Serialize(NetworkWriter writer) {}
            public void Deserialize(NetworkReader reader) {}
        }

        [Test]
        public void TestEmptyByteArray()
        {
            ByteArraySegmentMessage message = new ByteArraySegmentMessage
            {
                array = new ArraySegment<byte>(new byte[0])
            };

            byte[] data = MessagePacker.Pack(message);

            ByteArraySegmentMessage unpacked = MessagePacker.Unpack<ByteArraySegmentMessage>(data);

            Assert.IsNotNull(unpacked.array.Array);
            Assert.That(unpacked.array.Count, Is.EqualTo(0));
        }

        public static ArraySegment<int> SampleReader(NetworkReader reader)
        {
            int length = reader.ReadPackedInt32();
            int[] array = new int[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = reader.ReadPackedInt32();
            }

            return new ArraySegment<int>(array);
        }

        [Test]
        public void TestNullByteArray()
        {
            ByteArraySegmentMessage message = new ByteArraySegmentMessage
            {
                array = default
            };

            byte[] data = MessagePacker.Pack(message);

            ByteArraySegmentMessage unpacked = MessagePacker.Unpack<ByteArraySegmentMessage>(data);

            Assert.IsNull(unpacked.array.Array);
            Assert.That(unpacked.array.Offset, Is.EqualTo(0));
            Assert.That(unpacked.array.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestSegmentByteArray()
        {
            byte[] sourcedata = { 0, 1, 2, 3, 4, 5, 6 };

            ByteArraySegmentMessage message = new ByteArraySegmentMessage
            {
                array = new ArraySegment<byte>(sourcedata, 3, 2)
            };

            byte[] data = MessagePacker.Pack(message);

            ByteArraySegmentMessage unpacked = MessagePacker.Unpack<ByteArraySegmentMessage>(data);

            Assert.IsNotNull(unpacked.array.Array);
            Assert.That(unpacked.array.Count, Is.EqualTo(2));
            Assert.That(unpacked.array, Is.EquivalentTo(new byte[] { 3, 4 }));
        }
        #endregion

        #region ArraySegment<int>

        struct IntArraySegmentMessage : IMessageBase
        {
            public ArraySegment<int> array;

            // Weaver will generate serialization
            public void Serialize(NetworkWriter writer) {}
            public void Deserialize(NetworkReader reader) {}
        }

        [Test]
        public void TestEmptyIntArray()
        {
            IntArraySegmentMessage message = new IntArraySegmentMessage
            {
                array = new ArraySegment<int>(new int[0])
            };

            byte[] data = MessagePacker.Pack(message);

            IntArraySegmentMessage unpacked = MessagePacker.Unpack<IntArraySegmentMessage>(data);

            Assert.IsNotNull(unpacked.array.Array);
            Assert.That(unpacked.array.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestNullIntArray()
        {
            IntArraySegmentMessage message = new IntArraySegmentMessage
            {
                array = default
            };

            byte[] data = MessagePacker.Pack(message);

            IntArraySegmentMessage unpacked = MessagePacker.Unpack<IntArraySegmentMessage>(data);

            Assert.That(unpacked.array.Offset, Is.EqualTo(0));
            Assert.That(unpacked.array.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestSegmentIntArray()
        {
            int[] sourcedata = { 0, 1, 2, 3, 4, 5, 6 };

            IntArraySegmentMessage message = new IntArraySegmentMessage
            {
                array = new ArraySegment<int>(sourcedata, 3, 2)
            };

            byte[] data = MessagePacker.Pack(message);

            IntArraySegmentMessage unpacked = MessagePacker.Unpack<IntArraySegmentMessage>(data);

            Assert.IsNotNull(unpacked.array.Array);
            Assert.That(unpacked.array.Count, Is.EqualTo(2));
            Assert.That(unpacked.array, Is.EquivalentTo(new int[] { 3, 4 }));
        }
        #endregion
    }
}
