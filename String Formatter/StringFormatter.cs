using System.Text;

namespace String_Formatter
{
    public class StringFormatter : IStringFormatter
    {
        public static readonly StringFormatter Shared = new StringFormatter();
        private readonly ExpressionCashe cashe;

        StringFormatter()
        {
            cashe = new ExpressionCashe();
        }

        private enum State            // -------------------------
        {                             // |   | S | I | N | { | } |      
            Error = 0,                // -------------------------      S - symbols without I, N, '{', '}'  
            Text = 1,                 // | 0 | 0 | 0 | 0 | 0 | 0 |      I - symbols that can be first symbol of identifier
            Identifier = 2,           // -------------------------      N - number
            OpenBracket = 3,          // | 1 | 1 | 1 | 1 | 3 | 4 |
            CloseBracket = 4          // -------------------------      
        }                             // | 2 | 0 | 2 | 2 | 0 | 1 |
                                      // -------------------------
                                      // | 3 | 0 | 2 | 0 | 1 | 0 |
                                      // -------------------------
                                      // | 4 | 0 | 0 | 0 | 0 | 1 |
                                      // -------------------------

        private string identifierFirst = "_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private string numbers = "0123456789";

        public string Format(string template, object target)
        {
            var res = new StringBuilder();
            var identifire = new StringBuilder();
            var state = State.Text;

            for (int i = 0; i < template.Length; i++)
            {
                switch (state)  // [state, input]
                {
                    case State.Text: 
                        {
                            // [1, '{']
                            if (template[i] == '{')
                            {
                                res.Append(template[i]);
                                state = State.OpenBracket;
                            }
                            // [1, '}']
                            else if (template[i] == '}')
                            {
                                res.Append(template[i]);
                                state = State.CloseBracket;
                            }
                            // [1, S], [1, I], [1, N]
                            else
                            {
                                res.Append(template[i]);
                                state = State.Text;
                            }
                            break;
                        }
                    case State.Identifier: 
                        { 
                            // [2, I], [2, N]
                            if (identifierFirst.Contains(template[i]) || numbers.Contains(template[i]))
                            {
                                identifire.Append(template[i]);
                                state = State.Identifier;
                            }
                            // [2, '}']
                            else if (template[i] == '}')
                            {
                                string? strIdentifire = cashe.FindElement(identifire.ToString(), target);
                                if (strIdentifire == null) strIdentifire = cashe.AddElement(identifire.ToString(), target);
                                if (strIdentifire != null)
                                {
                                    res.Append(strIdentifire);
                                    state = State.Text;
                                }
                                else throw new ArgumentException($"Incorrect identifire {identifire.ToString()} at {i - identifire.Length + 1}");
                            }
                            // [2, S], [2, '{']
                            else throw new ArgumentException($"Incorrect symbol {template[i]} at {i}");
                            break; 
                        }
                    case State.OpenBracket: 
                        { 
                            // [3, I]
                            if (identifierFirst.Contains(template[i]))
                            {
                                res.Remove(res.Length - 1, 1);
                                identifire.Clear();
                                identifire.Append(template[i]);
                                state = State.Identifier;
                            }
                            // [3, '{']
                            else if (template[i] == '{')
                            {
                                state = State.Text;
                            }
                            // [3, S], [3, N], [3, '}']
                            else throw new ArgumentException($"Incorrect symbol {template[i]} at {i}");
                            break; 
                        }
                    case State.CloseBracket:
                        { 
                            // [4, '}']
                            if (template[i] == '}')
                            {
                                state = State.Text;
                            }
                            // [4, S], [4, I], [4, N], [4, '{']
                            else throw new ArgumentException($"Incorrect symbol {template[i]} at {i}");
                            break; 
                        }
                }
            }
            if (state != State.Text) throw new ArgumentException("Incorrect count of brackets");
            return res.ToString();
        }
    }
}
