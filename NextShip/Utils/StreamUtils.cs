using System;
using NextShip.Utilities.Attributes;
using System.IO;
using System.Runtime.CompilerServices;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace NextShip.Utils;

public static class StreamUtils
{
    // form reactor
    public static byte[] ReadFully(this Stream input)
    {
        using var ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
    
    
    [Il2CppRegister]
    public class StreamWrapper : Il2CppSystem.IO.Stream
    {
        private readonly Stream _stream;

        /// <inheritdoc />
        public StreamWrapper(IntPtr pointer) : base(pointer)
        {
            throw new NotSupportedException("This shouldn't ever be called because StreamWrapper is injected from managed side");
        }

        /// <inheritdoc />
        public StreamWrapper(Stream stream) : base(ClassInjector.DerivedConstructorPointer<StreamWrapper>())
        {
            ClassInjector.DerivedConstructorBody(this);
            _stream = stream;
        }

        [HideFromIl2Cpp]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Span<byte> GetSpan(Il2CppStructArray<byte> buffer, int offset, int count)
        {
            var rawBuffer = (byte*) buffer.Pointer + 4 * IntPtr.Size;
            return new Span<byte>(rawBuffer + offset, count);
        }

        /// <inheritdoc />
        public override int Read(Il2CppStructArray<byte> buffer, int offset, int count)
        {
            return _stream.Read(GetSpan(buffer, offset, count));
        }

        /// <inheritdoc />
        public override void Write(Il2CppStructArray<byte> buffer, int offset, int count)
        {
            _stream.Write(GetSpan(buffer, offset, count));
        }

        /// <inheritdoc />
        public override void Flush()
        {
            _stream.Flush();
        }

        /// <inheritdoc />
        public override void Close()
        {
            _stream.Close();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _stream.Dispose();
        }

        /// <inheritdoc />
        public override long Seek(long offset, Il2CppSystem.IO.SeekOrigin origin)
        {
            return _stream.Seek(offset, origin switch
            {
                Il2CppSystem.IO.SeekOrigin.Begin => SeekOrigin.Begin,
                Il2CppSystem.IO.SeekOrigin.Current => SeekOrigin.Current,
                Il2CppSystem.IO.SeekOrigin.End => SeekOrigin.End,
                _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null),
            });
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        /// <inheritdoc />
        public override bool CanRead => _stream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => _stream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => _stream.CanWrite;

        /// <inheritdoc />
        public override long Length => _stream.Length;

        /// <inheritdoc />
        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }
    }

    /// <summary>
    /// Wraps a <see cref="System.IO.Stream"/> into a <see cref="Il2CppSystem.IO.Stream"/>.
    /// </summary>
    /// <param name="stream">The stream to wrap.</param>
    /// <returns>A <see cref="StreamWrapper"/> for the specified <paramref name="stream"/>.</returns>
    public static StreamWrapper AsIl2Cpp(this Stream stream) => new(stream);
}