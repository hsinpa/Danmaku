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

        public bool debugMode = false;

        public MathParser() {
            _tokenizer = new Tokenizer();
            _shuntinYard = new ShuntingYard();
            _shuntinYardParser = new ShuntingYardParser();
            _customLookupTable = new Dictionary<string, float>();
        }

        /// <summary>
        /// Global Variable lookuptable
        /// </summary>
        /// <param name="p_lookupTable"></param>
        public void SetVariableLookUpTable(Dictionary<string, float> p_lookupTable) {
            _customLookupTable = p_lookupTable;
        }

        /// <summary>
        /// Return ShuntingYard symbol for later use.
        /// </summary>
        /// <param name="raw_syntax">string of math expression</param>
        /// <param name="debug_mode">Want more debug info</param>
        /// <returns></returns>
        public List<Token> ParseMathExpression(string raw_syntax)
        {
            string newSyntax = ReplaceVariable(StaticDataSet.PredefineVariableTable, raw_syntax);
                   newSyntax = raw_syntax.ToLower();

            var tokens = _tokenizer.Parse(newSyntax);

            var tokenList = _shuntinYard.Parse(tokens);

            if (debugMode)
                TokenToStringLog(tokens);

            if (debugMode)
                Debug.Log("Answer " + ParseShuntingYardToken(tokenList));

            return tokenList;
        }

        /// <summary>
        /// Solve for answer
        /// </summary>
        /// <param name="p_shuntinYardTokens">Parsed ShuntinYard Tokens</param>
        /// <param name="p_varaibleLookupTable">(Optional) LookupTable for varaible, will override the previous lookuptable data</param>
        /// <returns></returns>
        public float ParseShuntingYardToken(List<Token> p_shuntinYardTokens, Dictionary<string, float> p_varaibleLookupTable = null) {
            if (p_varaibleLookupTable != null)
                _customLookupTable = p_varaibleLookupTable;

            p_shuntinYardTokens = ReplaceVariable(_customLookupTable, p_shuntinYardTokens);

            if (debugMode)
                RPNToStringLog(p_shuntinYardTokens);

            return _shuntinYardParser.Parse(p_shuntinYardTokens);
        }

        private List<Token> ReplaceVariable(Dictionary<string, float> lookupTable, List<Token> tokens) {
            if (lookupTable == null)
                return tokens;

            for (int i = 0; i < tokens.Count; i++) {
                Token token = tokens[i];
                if (token._type == Token.Types.Variable && lookupTable.ContainsKey(token._value))
                {
                    //tokens[i].Set(lookupTable[token._value].ToString(), Token.Types.Number);
                    tokens[i] = new Token(lookupTable[token._value].ToString(), Token.Types.Number);
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

        public void TokenToStringLog(List<Token> tokens)
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