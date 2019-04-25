using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public class MathParser
    {
        Tokenizer _tokenizer;
        ShuntingYard _shuntinYard;
        ShuntingYardParser _shuntinYardParser;

        Dictionary<string, float> _customLookupTable;

        public MathParser() {
            _tokenizer = new Tokenizer();
            _shuntinYard = new ShuntingYard();
            _shuntinYardParser = new ShuntingYardParser();
            _customLookupTable = new Dictionary<string, float>();
        }

        public void SetVariableLookUpTable(Dictionary<string, float> p_lookupTable) {
            _customLookupTable = p_lookupTable;
        }

        public void Parse(string raw_syntax, bool debug_mode = false)
        {
            string newSyntax = ReplaceVariable(StaticDataSet.PredefineVariableTable, raw_syntax);
                   newSyntax = raw_syntax.ToLower();

            var tokens = _tokenizer.Parse(newSyntax);
                tokens = ReplaceVariable(_customLookupTable, tokens);

            var tokenList = _shuntinYard.Parse(tokens);

            if (debug_mode)
                TokenToStringLog(tokens);

            if (debug_mode)
                RPNToStringLog(tokenList);
            //Debug.Log("Answer " + _shuntinYardParser.Parse(tokenList));
            //rpn_tokens.Render();
        }


        private List<Token> ReplaceVariable(Dictionary<string, float> lookupTable, List<Token> tokens) {
            if (lookupTable == null)
                return tokens;

            for (int i = 0; i < tokens.Count; i++) {
                Token token = tokens[i];
                if (token._type == Token.Types.Variable && lookupTable.ContainsKey(token._value))
                {
                    float r_value = lookupTable[token._value];

                    token._type = Token.Types.Number;
                    token._value = r_value.ToString();
                }
            }
            return tokens;
        }

        private string ReplaceVariable(Dictionary<string, float> lookupTable, string p_raw_input)
        {
            foreach (KeyValuePair<string, float> predefineTable in lookupTable)
                p_raw_input = p_raw_input.Replace(predefineTable.Key, predefineTable.Value.ToString());

            return p_raw_input;
        }

        private void TokenToStringLog(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                Debug.Log(i + " => " + tokens[i]._type + "(" + tokens[i]._value + ")");
            }
        }

        private void RPNToStringLog(List<Token> tokens)
        {
            string groupString = "";
            foreach (Token t in tokens)
            {
                groupString += t._value + " ";
            }

            Debug.Log(groupString);
        }
    }
}