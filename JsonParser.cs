using Newtonsoft.Json.Linq;
//Copied this parser from  https://github.com/snatch-dev/Convey/blob/master/src/Convey.Secrets.Vault/src/Convey.Secrets.Vault/JsonParser.cs
namespace AspNetCoreVaultIntegration
{
    internal sealed class JsonParser
    {
        private readonly IDictionary<string, string> _mappings = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly Stack<string> _stack = new Stack<string>();

        private string _currentPath;

        public IDictionary<string, string> Parse(JObject jObject)
        {
            VisitJObject(jObject);
            return _mappings;
        }

        private void VisitJObject(JObject jObject)
        {
            foreach (JProperty item in jObject.Properties())
            {
                EnterContext(item.Name);
                VisitProperty(item);
                ExitContext();
            }
        }

        private void VisitProperty(JProperty property)
        {
            VisitToken(property.Value);
        }

        private void VisitToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    VisitJObject(token.Value<JObject>());
                    break;
                case JTokenType.Array:
                    VisitArray(token.Value<JArray>());
                    break;
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Null:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                    VisitPrimitive(token);
                    break;
                default:
                    throw new FormatException($"Invalid JSON token: {token}");
            }
        }

        private void VisitArray(JArray array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                EnterContext(i.ToString());
                VisitToken(array[i]);
                ExitContext();
            }
        }

        private void VisitPrimitive(JToken data)
        {
            string currentPath = _currentPath;
            if (_mappings.ContainsKey(currentPath))
            {
                throw new FormatException("Duplicated key: '" + currentPath + "'");
            }

            _mappings[currentPath] = data.ToString();
        }

        private void EnterContext(string context)
        {
            _stack.Push(context);
            _currentPath = ConfigurationPath.Combine(_stack.Reverse());
        }

        private void ExitContext()
        {
            _stack.Pop();
            _currentPath = ConfigurationPath.Combine(_stack.Reverse());
        }
    }

}
