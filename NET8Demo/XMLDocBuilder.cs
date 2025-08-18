using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NET8Demo
{
    internal class XMLDocBuilder
    {
        static string sourceDir = @"C:\Code\Chizl\Libraries\AnyOS\Chizl.ColorExtension"; // Adjust this to your library path
        static string otherXMLDir = @$"{sourceDir}\docs";
        static string outputDir = @$"{sourceDir}\xmldocs";
        static List<string> ignoreList = new List<string>() { "\\net8demo\\", "\\netframeworkdemo\\", "\\packages\\", "\\obj\\", "\\xmldocs\\", "\\bin\\", "\\docs\\" };

        private static void LoadClassSyntax(CompilationUnitSyntax root, string file, bool importOld)
        {
            foreach (var classNode in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var className = classNode.Identifier.Text;
                var classXml = new XElement("class", new XAttribute("name", className));
                var interfacesXml = new XElement("interfaces");
                var propertiesXml = new XElement("properties");
                var methodsXml = new XElement("methods");

                // Class-level summary
                var classSummary = GetSummaryFromTrivia(classNode.GetLeadingTrivia());
                if (!string.IsNullOrEmpty(classSummary))
                    classXml.Add(new XElement("summary", classSummary));

                classXml.Add(new XElement("returns")); // Optional stub for consistency

                // Constructors → interfaces
                foreach (var ctor in classNode.Members.OfType<ConstructorDeclarationSyntax>())
                {
                    var summary = GetSummaryFromTrivia(ctor.GetLeadingTrivia());
                    var returns = GetReturnsFromTrivia(ctor.GetLeadingTrivia());

                    var interfaceName = ctor.Identifier.Text + string.Join("", ctor.ParameterList.Parameters.Select(p =>
                        p.Type?.ToString() switch
                        {
                            var s when s.Contains("Color") => "Color",
                            var s when s.Contains("int") => "Int",
                            var s when s.Contains("string") => "String",
                            _ => "Other"
                        }));

                    var ifaceXml = new XElement("interface", new XAttribute("name", interfaceName));
                    ifaceXml.Add(new XElement("summary", summary));
                    ifaceXml.Add(new XElement("returns", returns));

                    //check and get parameters if exists.
                    var paramBlock = GetParamsFromTrivia(ctor.GetLeadingTrivia());
                    if (paramBlock != null)
                        ifaceXml.Add(paramBlock);

                    interfacesXml.Add(ifaceXml);
                }

                // Properties
                foreach (var prop in classNode.Members.OfType<PropertyDeclarationSyntax>())
                {
                    var name = prop.Identifier.Text;
                    var summary = GetSummaryFromTrivia(prop.GetLeadingTrivia());
                    var returns = GetReturnsFromTrivia(prop.GetLeadingTrivia());

                    var propXml = new XElement("property", new XAttribute("name", name));
                    propXml.Add(new XElement("summary", summary));
                    propXml.Add(new XElement("returns", returns));
                    propertiesXml.Add(propXml);
                }

                // Methods
                foreach (var method in classNode.Members.OfType<MethodDeclarationSyntax>())
                {
                    var name = method.Identifier.Text;
                    var summary = GetSummaryFromTrivia(method.GetLeadingTrivia());
                    var returns = GetReturnsFromTrivia(method.GetLeadingTrivia());

                    var methodXml = new XElement("method", new XAttribute("name", name));
                    methodXml.Add(new XElement("summary", summary));
                    methodXml.Add(new XElement("returns", returns));

                    //check and get parameters if exists.
                    var paramBlock = GetParamsFromTrivia(method.GetLeadingTrivia());
                    if (paramBlock != null)
                        methodXml.Add(paramBlock);

                    methodsXml.Add(methodXml);
                }

                if (interfacesXml.HasElements)
                    classXml.Add(interfacesXml);
                if (methodsXml.HasElements)
                    classXml.Add(methodsXml);
                if (propertiesXml.HasElements)
                    classXml.Add(propertiesXml);

                var doc = new XDocument(new XElement("extradoc", classXml));
                var outFile = Path.Combine(outputDir, $"{className}.xml");

                doc.Save(outFile);
                Console.WriteLine($"✅ Saved: {outFile}");

                if (importOld)
                {
                    var oldXMLFile = Path.GetFileNameWithoutExtension(file);
                    oldXMLFile = Path.Combine(otherXMLDir, $"{oldXMLFile}.xml");
                    if (File.Exists(oldXMLFile))
                    {
                        var docOld = new XmlDocument();
                        var docNew = new XmlDocument();

                        docOld.Load(oldXMLFile);
                        docNew.Load(outFile);

                        var par = docNew.ParentNode;
                        var noTp = docNew.NodeType;
                        var chl = docNew.ChildNodes;

                        foreach (XmlNode childEl in docOld.DocumentElement.ChildNodes)
                        {
                            var newNode = docNew.ImportNode(childEl, true);
                            docNew.DocumentElement.AppendChild(newNode);
                        }

                        docNew.Save(outFile);
                    }
                }
            }
        }
        private static void LoadStructSyntax(CompilationUnitSyntax root, string file, bool importOld)
        {
            foreach (var classNode in root.DescendantNodes().OfType<StructDeclarationSyntax>())
            {
                var className = classNode.Identifier.Text;
                var classXml = new XElement("class", new XAttribute("name", className));
                var interfacesXml = new XElement("interfaces");
                var propertiesXml = new XElement("properties");
                var methodsXml = new XElement("methods");

                // Class-level summary
                var classSummary = GetSummaryFromTrivia(classNode.GetLeadingTrivia());
                if (!string.IsNullOrEmpty(classSummary))
                    classXml.Add(new XElement("summary", classSummary));

                classXml.Add(new XElement("returns")); // Optional stub for consistency

                // Constructors → interfaces
                foreach (var ctor in classNode.Members.OfType<ConstructorDeclarationSyntax>())
                {
                    var summary = GetSummaryFromTrivia(ctor.GetLeadingTrivia());
                    var returns = GetReturnsFromTrivia(ctor.GetLeadingTrivia());

                    var interfaceName = ctor.Identifier.Text + string.Join("", ctor.ParameterList.Parameters.Select(p =>
                        p.Type?.ToString() switch
                        {
                            var s when s.Contains("Color") => "Color",
                            var s when s.Contains("int") => "Int",
                            var s when s.Contains("string") => "String",
                            _ => "Other"
                        }));

                    var ifaceXml = new XElement("interface", new XAttribute("name", interfaceName));
                    ifaceXml.Add(new XElement("summary", summary));
                    ifaceXml.Add(new XElement("returns", returns));

                    //check and get parameters if exists.
                    var paramBlock = GetParamsFromTrivia(ctor.GetLeadingTrivia());
                    if (paramBlock != null)
                        ifaceXml.Add(paramBlock);

                    interfacesXml.Add(ifaceXml);
                }

                // Properties
                foreach (var prop in classNode.Members.OfType<PropertyDeclarationSyntax>())
                {
                    var name = prop.Identifier.Text;
                    var summary = GetSummaryFromTrivia(prop.GetLeadingTrivia());
                    var returns = GetReturnsFromTrivia(prop.GetLeadingTrivia());

                    var propXml = new XElement("property", new XAttribute("name", name));
                    propXml.Add(new XElement("summary", summary));
                    propXml.Add(new XElement("returns", returns));
                    propertiesXml.Add(propXml);
                }

                // Methods
                foreach (var method in classNode.Members.OfType<MethodDeclarationSyntax>())
                {
                    var name = method.Identifier.Text;
                    var summary = GetSummaryFromTrivia(method.GetLeadingTrivia());
                    var returns = GetReturnsFromTrivia(method.GetLeadingTrivia());

                    var methodXml = new XElement("method", new XAttribute("name", name));
                    methodXml.Add(new XElement("summary", summary));
                    methodXml.Add(new XElement("returns", returns));

                    //check and get parameters if exists.
                    var paramBlock = GetParamsFromTrivia(method.GetLeadingTrivia());
                    if (paramBlock != null)
                        methodXml.Add(paramBlock);

                    methodsXml.Add(methodXml);
                }

                if (interfacesXml.HasElements)
                    classXml.Add(interfacesXml);
                if (methodsXml.HasElements)
                    classXml.Add(methodsXml);
                if (propertiesXml.HasElements)
                    classXml.Add(propertiesXml);

                var doc = new XDocument(new XElement("extradoc", classXml));
                var outFile = Path.Combine(outputDir, $"{className}.xml");

                doc.Save(outFile);
                Console.WriteLine($"✅ Saved: {outFile}");

                if (importOld)
                {
                    var oldXMLFile = Path.GetFileNameWithoutExtension(file);
                    oldXMLFile = Path.Combine(otherXMLDir, $"{oldXMLFile}.xml");
                    if (File.Exists(oldXMLFile))
                    {
                        var docOld = new XmlDocument();
                        var docNew = new XmlDocument();

                        docOld.Load(oldXMLFile);
                        docNew.Load(outFile);

                        foreach (XmlNode oChild1 in docOld.DocumentElement.ChildNodes)
                        {
                            var val = oChild1.Value;
                            var nChild2 = docNew.ImportNode(oChild1, true);
                            docNew.DocumentElement.AppendChild(nChild2);
                            if (oChild1.HasChildNodes)
                            {
                                foreach (XmlNode oChild2 in oChild1.ChildNodes)
                                {
                                    var newNode = docNew.ImportNode(oChild2, true);
                                    docNew.DocumentElement.AppendChild(newNode);
                                }
                            }
                            else
                                docNew.DocumentElement.AppendChild(nChild2);
                        }

                        docNew.Save(outFile);
                    }
                }
            }
        }
        public static void CreateDocs(bool importOld = false)
        {
            Console.InputEncoding = Encoding.UTF8;
            Directory.CreateDirectory(outputDir);
            var syntaxKind = typeof(StructDeclarationSyntax);
            // Process all .cs files in the directory
            foreach (var file in Directory.GetFiles(sourceDir, "*.cs", SearchOption.AllDirectories))
            {
                //if (ignoreList.Contains(file.ToLower()))
                if (ignoreList.Where(w=>file.ToLower().Contains(w)).Count()>0)
                    continue;

                var code = File.ReadAllText(file);
                var tree = CSharpSyntaxTree.ParseText(code);
                var root = tree.GetCompilationUnitRoot();
                var clsNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                var stcNodes = root.DescendantNodes().OfType<StructDeclarationSyntax>();

                if (clsNodes != null && clsNodes.Count() > 0)
                    LoadClassSyntax(root, file, importOld);
                else if (stcNodes != null && stcNodes.Count() > 0)
                    LoadStructSyntax(root, file, importOld);

            }

            Console.ReadKey(true);
        }

        static string GetSummaryFromTrivia(SyntaxTriviaList trivia)
        {
            var doc = trivia.Select(x => x.GetStructure())
                            .OfType<DocumentationCommentTriviaSyntax>()
                            .FirstOrDefault();

            if (doc == null) return "";

            var summaryElement = doc.ChildNodes()
                .OfType<XmlElementSyntax>()
                .FirstOrDefault(e => e.StartTag.Name.LocalName.Text == "summary");

            if (summaryElement == null) return "";

            var lines = summaryElement.Content
                .Select(c => c.ToString().Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            return string.Join(" ", lines).Replace("///", "").Trim();
        }

        static string GetReturnsFromTrivia(SyntaxTriviaList trivia)
        {
            var doc = trivia.Select(x => x.GetStructure())
                            .OfType<DocumentationCommentTriviaSyntax>()
                            .FirstOrDefault();

            if (doc == null) return "";

            var returnsElement = doc.ChildNodes()
                .OfType<XmlElementSyntax>()
                .FirstOrDefault(e => e.StartTag.Name.LocalName.Text == "returns");

            if (returnsElement == null) return "";

            var lines = returnsElement.Content
                .Select(c => c.ToString().Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            return string.Join(" ", lines).Replace("///", "").Trim();
        }

        public static XElement GetParamsFromTrivia(SyntaxTriviaList trivia)
        {
            var doc = trivia.Select(x => x.GetStructure())
                            .OfType<DocumentationCommentTriviaSyntax>()
                            .FirstOrDefault();

            if (doc == null)
                return null;

            var paramElements = doc.ChildNodes()
                .OfType<XmlElementSyntax>()
                .Where(e => e.StartTag.Name.LocalName.Text == "param");

            if (!paramElements.Any())
                return null;

            var paramsXml = new XElement("params");

            foreach (var p in paramElements)
            {
                string name = p.StartTag.Attributes
                    .OfType<XmlNameAttributeSyntax>()
                    .FirstOrDefault()?.Identifier?.Identifier.Text ?? "unknown";

                string content = string.Join(" ", p.Content.Select(c => c.ToString().Trim()));

                var paramXml = new XElement("param",
                    new XAttribute("name", name),
                    new XElement("summary", content));

                paramsXml.Add(paramXml);
            }

            return paramsXml;
        }
    }
}
