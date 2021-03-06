﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using YC.ReSharper.AbstractAnalysis.Plugin.Highlighting.Dynamic;
using YC.SDK.ReSharper;

namespace YC.ReSharper.AbstractAnalysis.Plugin.Highlighting
{
    public class HighlightingProcess : IDaemonStageProcess
    {
        private Action<DaemonStageResult> myCommiter;
        private IContextBoundSettingsStore mySettingsStore;

        private static Helper.ReSharperHelper<DocumentRange, ITreeNode> YcProcessor = Helper.ReSharperHelper<DocumentRange, ITreeNode>.Instance;

        public IDaemonProcess DaemonProcess { get; private set; }
        public IHighlightingConsumer Consumer
        {
            get
            {
                return new DefaultHighlightingConsumer(this, mySettingsStore);
            }
        }

        private ICSharpFile csFile;
        public ICSharpFile CSharpFile
        {
            get
            {
                if (csFile == null)
                    csFile = GetCsFile();
                return csFile;
            }
        }

        private ICSharpFile GetCsFile()
        {
            IPsiServices psiServices = DaemonProcess.SourceFile.GetPsiServices();
            return psiServices.Files.GetDominantPsiFile<CSharpLanguage>(DaemonProcess.SourceFile) as ICSharpFile;
        }

        public HighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
        {
            DaemonProcess = process;
            mySettingsStore = settingsStore;
        }

        public void Update(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
        {
            DaemonProcess = process;
            mySettingsStore = settingsStore;
        }

        public void Execute(Action<DaemonStageResult> commiter)
        {
            if (CSharpFile == null)
                return;

            myCommiter = commiter;
            UpdateHandler();

            ExistingTreeNodes.ClearExistingTree(DaemonProcess.Document);
            var errors = YcProcessor.Process(CSharpFile);
            OnErrors(errors);
            // remove all old highlightings
            //if (DaemonProcess.FullRehighlightingRequired)
            //myCommiter(new DaemonStageResult(EmptyArray<HighlightingInfo>.Instance));
        }

        private void UpdateHandler()
        {
            Handler.Process = this;
        }

        private void OnErrors(YC.SDK.ReSharper.Helper.ProcessErrors errors)
        {
            var highlightings = new List<HighlightingInfo>();

            var parserErrors = errors.ParserErrors.Info;
            if (parserErrors.Count > 0)
            {
                highlightings.AddRange(from error in parserErrors
                                       select new HighlightingInfo(error.Item2, new ErrorWarning("Syntax error. Unexpected token " /*+ error.Item1*/)));
            }

            var lexerErrors = errors.LexerErrors.Info;
            if (lexerErrors.Count > 0)
            {
                highlightings.AddRange(from error in lexerErrors
                                       select new HighlightingInfo(error.Item2, new ErrorWarning("Unexpected symbol: " + error.Item1 + ".")));
            }

            var semanticErrors = errors.SemanticErrors.Info;
            if (semanticErrors.Count > 0)
            {
                highlightings.AddRange(from error in semanticErrors
                                       select new HighlightingInfo(error.Item2, new ErrorWarning("Semantic error. Symbol: " + error.Item1 + ".")));
            }
            //var highlightings = (from e in errors.Item2 select new HighlightingInfo(e.Item2, new ErrorWarning())).Concat(
            //                    from e in errors.Item1 select new HighlightingInfo(e.Item2, new ErrorWarning("Unexpected symbol: " + e.Item1 + ".")));
            DoHighlighting(new DaemonStageResult(highlightings));
        }

        public void DoHighlighting(DaemonStageResult result)
        {
            myCommiter(result);
        }
    }
}
