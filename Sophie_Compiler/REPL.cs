﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Compiler.CodeAnalysis;
using Compiler.CodeAnalysis.Syntax;
using Compiler.CodeAnalysis.Text;

namespace Sophie_Compiler;

internal abstract class REPL
{
    private List<string> _submissionHistory = new();
    private int _submissionHistoryIndex;
    private Compilation _previous;
    private bool _showTree;
    private bool _showProgram;
    private Dictionary<VariableSymbol, object> _variables;
    private bool _done;

    public void Run()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            var text = EditSubmission();

            if (string.IsNullOrEmpty(text))
                return;

            if (!text.Contains(Environment.NewLine) && text.StartsWith("#"))
                EvaluateMetaCommand(text);
            else
                EvaluateSubmission(text);
            
            _submissionHistory.Add(text);
            _submissionHistoryIndex = 0;
        }
    }

    protected virtual void ClearHistory()
    {
        _submissionHistory.Clear();
    }

    private string EditSubmissionOld()
    {
        StringBuilder textBuilder = new StringBuilder();
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (textBuilder.Length == 0)
                Console.Write("> ");
            else
                Console.Write("| ");
            Console.ResetColor();
            var input = Console.ReadLine();
            var isBlank = string.IsNullOrEmpty(input);

            if (textBuilder.Length == 0)
            {
                if (isBlank)
                {
                    return null;
                }
                else if (input.StartsWith("#"))
                {
                    EvaluateMetaCommand(input);
                    continue;
                }
            }

            textBuilder.AppendLine(input);
            var inputText = textBuilder.ToString();
            if (!IsCompleteSubmission(inputText))
                continue;

            return inputText;
        }
    }

    protected virtual void EvaluateMetaCommand(string input)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Invalid command: {input}.");
        Console.ResetColor();
    }

    protected abstract bool EvaluateSubmission(string inputText);


    protected abstract bool IsCompleteSubmission(string text);

    private string EditSubmission()
    {
        _done = false;
        var document = new ObservableCollection<string>() { "" };
        var view = new SubmissionView(document);

        while (!_done)
        {
            var key = Console.ReadKey(true);
            HandleKey(key, document, view);
        }

        Console.WriteLine();


        return string.Join(Environment.NewLine, document);
    }

    private void HandleKey(ConsoleKeyInfo key, ObservableCollection<string> document, SubmissionView view)
    {
        if (key.Modifiers == default(ConsoleModifiers))
        {
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    HandleEscape(document, view);
                    break;
                case ConsoleKey.Enter:
                    HandleEnter(document, view);
                    break;
                case ConsoleKey.LeftArrow:
                    HandleLeftArrow(document, view);
                    break;
                case ConsoleKey.RightArrow:
                    HandleRightArrow(document, view);
                    break;
                case ConsoleKey.UpArrow:
                    HandleUpArrow(document, view);
                    break;
                case ConsoleKey.DownArrow:
                    HandleDownArrow(document, view);
                    break;
                case ConsoleKey.Backspace:
                    HandleBackspace(document, view);
                    break;
                case ConsoleKey.Delete:
                    HandleDelete(document, view);
                    break;
                case ConsoleKey.Home:
                    HandleHome(document, view);
                    break;
                case ConsoleKey.End:
                    HandleEnd(document, view);
                    break;
                case ConsoleKey.Tab:
                    HandleTab(document, view);
                    break;
                case ConsoleKey.PageUp:
                    HandlePageUp(document, view);
                    break;
                case ConsoleKey.PageDown:
                    HandlePageDown(document, view);
                    break;
            }
        }
        else if (key.Modifiers == ConsoleModifiers.Control)
        {
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    HandleControlEnter(document, view);
                    break;
            }
        }

        if (key.KeyChar >= ' ')
            HandleTyping(document, view, key.KeyChar.ToString());
    }

    private void HandlePageDown(ObservableCollection<string> document, SubmissionView view)
    {
        _submissionHistoryIndex++;
        if (_submissionHistoryIndex > _submissionHistory.Count() - 1)
            _submissionHistoryIndex = _submissionHistory.Count() - 1;
        UpdateDocumentFromHistory(document, view);
    }

    private void HandlePageUp(ObservableCollection<string> document, SubmissionView view)
    {
        _submissionHistoryIndex--;
        if (_submissionHistoryIndex < 0)
            _submissionHistoryIndex = _submissionHistory.Count() - 1;
        UpdateDocumentFromHistory(document, view);
    }

    private void UpdateDocumentFromHistory(ObservableCollection<string> document, SubmissionView view)
    {
        document.Clear();
        var historyItem = _submissionHistory[_submissionHistoryIndex];
        var lines = historyItem.Split(Environment.NewLine);
        foreach (var line in lines)
            document.Add(line);

        view.CurrentLineIndex = document.Count - 1;
        view.CurrentCharacter = document[view.CurrentLineIndex].Length;
    }

    private void HandleEscape(ObservableCollection<string> document, SubmissionView view)
    {
        document[view.CurrentLineIndex] = string.Empty;
        view.CurrentCharacter = 0;
    }

    private void HandleTab(ObservableCollection<string> document, SubmissionView view)
    {
        const int TabWidth = 4;
        var start = view.CurrentCharacter;
        var remainingSpaces = TabWidth - start % TabWidth;

        var line = document[view.CurrentLineIndex];
        document[view.CurrentLineIndex] = line.Insert(start, new string(' ', remainingSpaces));
        view.CurrentCharacter += remainingSpaces;
    }

    private void HandleEnd(ObservableCollection<string> document, SubmissionView view)
    {
        view.CurrentCharacter = document[view.CurrentLineIndex].Length;
    }

    private void HandleHome(ObservableCollection<string> document, SubmissionView view)
    {
        view.CurrentCharacter = 0;
    }

    private void HandleDelete(ObservableCollection<string> document, SubmissionView view)
    {
        var index = view.CurrentLineIndex;
        var line = document[index];
        var start = view.CurrentCharacter;
        if (start >= line.Length)
            return;


        var before = line.Substring(0, start);
        var after = line.Substring(start + 1);
        document[index] = before + after;
    }

    private void HandleBackspace(ObservableCollection<string> document, SubmissionView view)
    {
        var start = view.CurrentCharacter;

        if (start == 0)
            return;
        var index = view.CurrentLineIndex;
        var line = document[index];
        var before = line.Substring(0, start - 1);
        var after = line.Substring(start);
        document[index] = before + after;
        view.CurrentCharacter--;
    }

    private void HandleControlEnter(ObservableCollection<string> document, SubmissionView view)
    {
        _done = true;
    }

    private void HandleUpArrow(ObservableCollection<string> document, SubmissionView view)
    {
        if (view.CurrentLineIndex > 0)
            view.CurrentLineIndex--;
    }

    private void HandleRightArrow(ObservableCollection<string> document, SubmissionView view)
    {
        var line = document[view.CurrentLineIndex];
        if (view.CurrentCharacter <= line.Length - 1)
            view.CurrentCharacter++;
    }

    private void HandleLeftArrow(ObservableCollection<string> document, SubmissionView view)
    {
        if (view.CurrentCharacter > 0)
            view.CurrentCharacter--;
    }

    private void HandleEnter(ObservableCollection<string> document, SubmissionView view)
    {
        var documentText = string.Join(Environment.NewLine, document);
        if (documentText.StartsWith("#") || IsCompleteSubmission(documentText))
        {
            _done = true;
            return;
        }

        document.Add(string.Empty);
        view.CurrentCharacter = 0;
        view.CurrentLineIndex = document.Count - 1;
    }


    private void HandleDownArrow(ObservableCollection<string> document, SubmissionView view)
    {
        if (view.CurrentLineIndex < document.Count - 1)
            view.CurrentLineIndex++;
    }

    private void HandleTyping(ObservableCollection<string> document, SubmissionView view, string text)
    {
        var lineIndex = view.CurrentLineIndex;
        var startIndex = view.CurrentCharacter;
        document[lineIndex] = document[lineIndex].Insert(startIndex, text);
        view.CurrentCharacter += text.Length;
    }

    private sealed class SubmissionView
    {
        private readonly ObservableCollection<string> _submissionDocument;
        private readonly int _cursorTop;
        private int _renderedLineCount;
        private int _currentLineIndex;
        private int _currentCharacter;

        public int CurrentLineIndex
        {
            get => _currentLineIndex;
            set
            {
                if (_currentLineIndex != value)
                {
                    _currentLineIndex = value;
                    _currentCharacter = Math.Min(_submissionDocument[_currentLineIndex].Length, _currentCharacter);
                    UpdateCursorPosition();
                }
            }
        }

        public int CurrentCharacter
        {
            get => _currentCharacter;
            set
            {
                if (_currentCharacter != value)
                {
                    _currentCharacter = value;
                    UpdateCursorPosition();
                }
            }
        }

        public SubmissionView(ObservableCollection<string> submissionDocument)
        {
            _submissionDocument = submissionDocument;
            _submissionDocument.CollectionChanged += SubmissionDocumentChanged;
            _cursorTop = Console.CursorTop;
            Render();
        }

        private void SubmissionDocumentChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Render();
        }

        private void Render()
        {
            Console.CursorVisible = false;

            var lineCount = 0;
            foreach (var line in _submissionDocument)
            {
                Console.SetCursorPosition(0, _cursorTop + lineCount);
                Console.ForegroundColor = ConsoleColor.Cyan;

                if (lineCount == 0)
                    Console.Write("> ");
                else
                    Console.Write("| ");
                Console.ResetColor();
                Console.Write(line);
                var remainingSpaces = new string(' ', Console.WindowWidth - line.Length);
                Console.WriteLine(remainingSpaces);
                lineCount++;
            }

            var numberOfBlankLines = _renderedLineCount - lineCount;
            if (numberOfBlankLines-- > 0)
            {
                var blankLine = new string(' ', Console.WindowWidth);
                while (numberOfBlankLines > 0)
                {
                    Console.WriteLine(blankLine);
                }
            }

            _renderedLineCount = lineCount;
            Console.CursorVisible = true;
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            Console.CursorTop = _cursorTop + _currentLineIndex;
            Console.CursorLeft = 2 + _currentCharacter;
        }
    }
}