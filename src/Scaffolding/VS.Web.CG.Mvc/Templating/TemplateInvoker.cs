using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TextTemplating;
using System.Reflection;
using Mono.TextTemplating;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.T4.RazorPages;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.T4;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templating
{
    /// <summary>
    /// Contains useful helper functions for running visual studio text transformation.
    /// For internal microsoft use only. Use <see cref="ITemplateInvoker"/>
    /// in custom code generators.
    /// </summary>
    internal class TemplateInvoker : ITemplateInvoker
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public TemplateInvoker(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes a code generator template to generate the code.
        /// </summary>
        /// <param name="templatePath">Full path of the template file.</param>
        /// <param name="templateParameters">Parameters for the template.
        /// These parameters can be accessed in text template using a parameter directive.
        /// The values passed in must be either serializable or 
        /// extend <see cref="System.MarshalByRefObject"/> type.</param>
        /// <returns>Generated code if there were no processing errors. Throws 
        /// <see cref="System.InvalidOperationException" /> otherwise.
        /// </returns>
        public string InvokeTemplate(string templatePath,
            IDictionary<string, object> templateParameters)
        {
            if (string.IsNullOrEmpty(templatePath))
            {
                //ExceptionUtil.ThrowStringEmptyArgumentException(nameof(templatePath));
            }

            if (templateParameters == null)
            {
                throw new ArgumentNullException(nameof(templateParameters));
            }

            /*            TemplateProcessingResult result = new TemplateProcessingResult();

                        TextTemplatingEngineHost engineHost = new TextTemplatingEngineHost(_serviceProvider);
                        foreach (KeyValuePair<string, object> entry in templateParameters)
                        {
                            if (entry.Value == null)
                            {
                                throw new InvalidOperationException(
                                    string.Format(CultureInfo.CurrentCulture,
                                        "ABCDE",
                                        entry.Key,
                                        templatePath));
                            }

                            engineHost.Session.Add(entry.Key, entry.Value);
                        }

                        var vsTextTemplating = new TemplatingEngine();
                        result.GeneratedText = vsTextTemplating.ProcessTemplate(
                            File.ReadAllText(templatePath),
                            engineHost);

                        return result;*/
            System.Diagnostics.Debugger.Launch();
            var host = new TextTemplatingEngineHost(_serviceProvider)
            {
                TemplateFile = "D:\\Stuff\\scaffolding\\src\\Scaffolding\\VS.Web.CG.Mvc\\Templates\\T4\\RazorPages\\RazorPageEmptyGenerator.tt"
            };

            var contextTemplate = new RazorPageEmptyGenerator
            {
                Host = host,
                Session = host.CreateSession()
            };

            contextTemplate.Session.Add("RazorPageClassName", "ClassName");
            contextTemplate.Session.Add("Namespace", "TestNamespace");

            string generatedCode = string.Empty;
            if (contextTemplate != null)
            {
                contextTemplate.Initialize();
                //generatedCode = new TemplatingEngine().ProcessTemplate(File.ReadAllText(contextTemplate), host);
                //CheckEncoding(host.OutputEncoding);
                //HandleErrors(host);

                //CompiledTemplate compiledEntityTypeTemplate = new TemplatingEngine().CompileTemplate(File.ReadAllText(host.TemplateFile), host);
                //generatedCode = compiledEntityTypeTemplate.Process();
                generatedCode = ProcessTemplate(contextTemplate);
            }
            return generatedCode;
        }

        private string ProcessTemplate(ITextTransformation transformation)
        {
            var output = transformation.TransformText();

            foreach (CompilerError error in transformation.Errors)
            {
                //_reporter.Write(error);
            }

            if (transformation.Errors.HasErrors)
            {
                //throw new OperationException(DesignStrings.ErrorGeneratingOutput(transformation.GetType().Name));
            }

            return output;
        }

    }
}
