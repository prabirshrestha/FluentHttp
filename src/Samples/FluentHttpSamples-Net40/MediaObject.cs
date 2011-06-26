
namespace FluentHttpSamples
{
    /// <summary>
    /// Represents a media object such as a photo or video.
    /// </summary>
    public class MediaObject
    {
        /// <summary>
        /// The value of the media object.
        /// </summary>
        private byte[] _value;

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Sets the value of the media object.
        /// </summary>
        /// <param name="value">The media object value.</param>
        /// <returns>Media Object</returns>
        public MediaObject SetValue(byte[] value)
        {
            _value = value;
            return this;
        }

        /// <summary>
        /// Gets the value of the media object.
        /// </summary>
        /// <returns>The value of the media object.</returns>
        public byte[] GetValue()
        {
            return _value;
        }
    }
}