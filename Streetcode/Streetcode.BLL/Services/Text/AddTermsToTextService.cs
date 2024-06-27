using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Interfaces.Text;
using Streetcode.BLL.Util;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Text
{
    public class AddTermsToTextService : ITextService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private List<int> _buffer;
        private HashSet<Term> _terms;
        private HashSet<RelatedTerm> _relatedTerms;
        private readonly StringBuilder _text = new StringBuilder();

        public AddTermsToTextService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _buffer = new List<int>();
            _terms = new HashSet<Term>(new IdComparer<Term>());
            _relatedTerms = new HashSet<RelatedTerm>(new IdComparer<RelatedTerm>());
            Pattern = new("(\\s)|(<[^>]*>)", RegexOptions.None, TimeSpan.FromMilliseconds(1000));
        }

        public Regex Pattern { get; private set; }

        public async Task<string> AddTermsTag(string text)
        {
            _text.Clear();
            var splittedText = Pattern.Split(text)
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToArray();

            if (splittedText[0].Contains("<p"))
            {
                var split = splittedText[0].Replace("<p", "<span");
                splittedText[0] = split;
            }

            HashSet<string> uniqueWords = new();

            foreach (var word in splittedText)
            {
                var (resultedWord, extras) = CleanWord(word);
                uniqueWords.Add(resultedWord.ToLower());
            }

            _terms = new HashSet<Term>(
                await _repositoryWrapper.TermRepository
                .GetAllAsync(t => uniqueWords.Contains(t.Title.ToLower())));

            _relatedTerms = new HashSet<RelatedTerm>(
                await _repositoryWrapper.RelatedTermRepository
                .GetAllAsync(
                rt => uniqueWords.Contains(rt.Word.ToLower()),
                rt => rt.Include(rt => rt.Term)));
                
            foreach (var word in splittedText)
            {
                if (word.Contains('<'))
                {
                    _text.Append(word);
                    continue;
                }

                var (resultedWord, extras) = CleanWord(word);
                Term? term = null;

                if (_terms.Any(x => x.Title.ToLower() == resultedWord.ToLower()))
                {
                    term = _terms.FirstOrDefault(x => x.Title.ToLower() == resultedWord.ToLower());
                }

                if (term == null)
                {
                    var buffer = await AddRelatedAsync(resultedWord);
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        resultedWord = buffer;
                    }
                }
                else
                {
                    if (!CheckInBuffer(term.Id))
                    {
                        resultedWord = MarkTermWithDescription(resultedWord, term.Description);
                        AddToBuffer(term.Id);
                    }
                }

                _text.Append(resultedWord + extras + ' ');
            }

            CLearBuffer();
            return _text.ToString();
        }

        private void AddToBuffer(int key) => _buffer.Add(key);

        private bool CheckInBuffer(int key) => _buffer.Contains(key);

        private void CLearBuffer() => _buffer.Clear();

        private static string MarkTermWithDescription(string word, string description) => $"<Popover><Term>{word}</Term><Desc>{description}</Desc></Popover>";

        private async Task<string> AddRelatedAsync(string clearedWord)
        {
            RelatedTerm? relatedTerm = null;

            if(_relatedTerms.Any(x => x.Word.ToLower() == clearedWord.ToLower()))
            {
                relatedTerm = _relatedTerms.FirstOrDefault(x => x.Word.ToLower() == clearedWord.ToLower());
            }

            if (relatedTerm == null || relatedTerm.Term == null || CheckInBuffer(relatedTerm.TermId))
            {
                return string.Empty;
            }

            AddToBuffer(relatedTerm.TermId);

            return MarkTermWithDescription(clearedWord, relatedTerm.Term.Description);
        }

        private (string _clearedWord, string _extras) CleanWord(string word)
        {
            var clearedWord = word.Split('.', ',').First();

            var extras = string.Empty;

            if (!word.Equals(clearedWord))
            {
                extras = new string(word.Except(clearedWord).ToArray());
            }

            return (clearedWord, extras);
        }
    }
}
