namespace Serilog.Exceptions.Test.Destructurers
{
    using System;
    using System.Collections;
    using Core;
    using Exceptions.Destructurers;
    using Xunit;

    public class UriDestructurerTest
    {
        private ReflectionBasedDestructurer destructurer;

        public UriDestructurerTest()
        {
            this.destructurer = new ReflectionBasedDestructurer();
        }

        [Fact]
        public void CanDestructureUriProperty()
        {
            const string uriValue = "http://localhost/property";
            var exception = new UriException("test", new Uri(uriValue));

            var propertiesBag = new ExceptionPropertiesBag(exception);
            this.destructurer.Destructure(exception, propertiesBag, null);

            var properties = propertiesBag.GetResultDictionary();
            var uriPropertyValue = properties[nameof(UriException.Uri)];
            Assert.IsType<string>(uriPropertyValue);
            Assert.Equal(uriValue, uriPropertyValue);
        }

        [Fact]
        public void CanDestructureDataItem()
        {
            const string uriValue = "http://localhost/data-item";
            var exception = new Exception("test")
            {
                Data =
                {
                    { "UriDataItem", new Uri(uriValue) }
                }
            };

            var propertiesBag = new ExceptionPropertiesBag(exception);
            this.destructurer.Destructure(exception, propertiesBag, null);

            var properties = propertiesBag.GetResultDictionary();
            var data = (IDictionary)properties[nameof(Exception.Data)];
            var uriDataValue = data["UriDataItem"];
            Assert.IsType<string>(uriDataValue);
            Assert.Equal(uriValue, uriDataValue);
        }

        public class UriException : Exception
        {
            public UriException(string message, Uri uri)
                : base(message)
            {
                this.Uri = uri;
            }

            public Uri Uri { get; }
        }
    }
}