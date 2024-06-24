using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Interfaces.Text;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Text
{
    public class AddTermsToTextService : ITextService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private List<int> _buffer;
        private HashSet<string> _notTerm;
        private HashSet<string> _notRelated;
        private readonly StringBuilder _text = new StringBuilder();

        public AddTermsToTextService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _buffer = new List<int>();
            _notTerm = new HashSet<string>();
            _notRelated = new HashSet<string>();
            Pattern = new("(\\s)|(<[^>]*>)", RegexOptions.None, TimeSpan.FromMilliseconds(1000));
        }

        public Regex Pattern { get; private set; }

        // TODO: it should be optimized
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

            foreach (var word in splittedText)
            {
                if (word.Contains('<'))
                {
                    _text.Append(word);
                    continue;
                }

                var (resultedWord, extras) = CleanWord(word);
                Term? term = null;
                if (!_notTerm.Contains(resultedWord))
                {
                    term = await _repositoryWrapper.TermRepository
                    .GetFirstOrDefaultAsync(
                        t => t.Title.ToLower().Equals(resultedWord.ToLower()));
                }

                if (term == null)
                {
                    _notTerm.Add(resultedWord);
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
            if (!_notRelated.Contains(clearedWord))
            {
                 relatedTerm = await _repositoryWrapper.RelatedTermRepository
                .GetFirstOrDefaultAsync(
                rt => rt.Word.ToLower().Equals(clearedWord.ToLower()),
                rt => rt.Include(rt => rt.Term));
            }

            if (relatedTerm == null || relatedTerm.Term == null || CheckInBuffer(relatedTerm.TermId))
            {
                _notRelated.Add(clearedWord);
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
