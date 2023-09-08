using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace ChangeStreamsApp.Models;

public class SearchIndexBO
{

    string _ns;
    const string analyzer = "lucene.keyword";

    List<FieldBO> _fields;
    public SearchIndexBO(string ns, List<FieldBO> fields)
    {
        _ns = ns;
        _fields = fields;
    }

    private static BsonDocument GenerateCustomAnalyzer() {

        BsonDocument customAnalyzer = new BsonDocument
        {
            new BsonElement("charFilters", new BsonArray()),
            new BsonElement("name", "keyword_lowercaser"),
            new BsonElement("tokenFilters", new BsonArray(new[] {new BsonDocument(new BsonElement("type", "lowercase"))})),
            new BsonElement("tokenizer", new BsonDocument(new BsonElement("type", "keyword")))
        };

        return customAnalyzer;
    }

    private BsonDocument _generateFields() {
        BsonDocument fields = new BsonDocument();

        foreach (var field in _fields)
        {
            fields.Add(new BsonElement(field.AttrName, new BsonDocument{
                new BsonElement("analyzer", "keyword_lowercaser"),
                new BsonElement("searchAnalyzer", "keyword_lowercaser"),
                new BsonElement("type", "string"),
            }));
        }

        return fields;
    }

    public BsonDocument getSearchIndex() {

        var cmd = new BsonDocument();

        cmd.Add(new BsonElement("createSearchIndexes", _ns.Split('.')[1]));

        var inx = new BsonDocument();
        inx.Add(new BsonElement("name", Regex.Replace(_ns+"_Search", @"\.", "_")));

        var inxDefinition = new BsonDocument
        {
            {"analyzer", analyzer},
            {"searchAnalyzer", analyzer},
            {"mappings", new BsonDocument
                {
                    {"dynamic", false},
                    {"fields", _generateFields()},
                }
            },
            {"analyzers", new BsonArray(new[] {GenerateCustomAnalyzer()})}
        };

        inx.Add(new BsonElement("definition", new BsonDocument(inxDefinition)));

        cmd.Add(new BsonElement("indexes", new BsonArray(new[] { inx })));

        return cmd;
    }

}