using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace UnityToJsonMapConverter
{
    class UnityGameObject
    {
        public string Id { get; set; }

        public List<string> ComponentIds { get; set; }

        public UnityGameObject(string id, YamlMappingNode mappings)
        {
            Id = id;
            ComponentIds = new List<string>();

            var components = (YamlSequenceNode)mappings.Children[new YamlScalarNode("m_Component")];

            foreach(YamlMappingNode component in components)
            {
                var node = (YamlScalarNode)(((YamlMappingNode)component.Children.Values.First()).Children.Values.Last());
                ComponentIds.Add(node.Value);
            }


            //var sequence = (YamlSequenceNode)mappingNode.Value;
            //foreach (YamlMappingNode sequenceItem in sequence)
            //{
            //    sequenceItem

            //                foreach (var mappingValue in sequenceItem)
            //    {
            //        var typeId = (YamlScalarNode)mappingValue.Key;
            //    }
            //}
        }
    }

    class TransformComponent
    {
        public string Id { get; set; }
    }

    class Program
    {
        static List<UnityGameObject> _gameObjects = new List<UnityGameObject>();

        static void Main(string[] args)
        {
            try
            {
                var commandLine = new CommandLineParser(args);

                using (var textReader = new StreamReader(commandLine.UnityFilePath))
                {
                    var unitySceneFile = new YamlStream();
                    unitySceneFile.Load(textReader);
                    ConvertToJsonFile(unitySceneFile, commandLine.JsonMapFilePath);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void ConvertToJsonFile(YamlStream unityScene, string jsonMapFilePath)
        {
            foreach(var document in unityScene.Documents)
            {
                var rootNode = (YamlMappingNode)document.RootNode;

                foreach(var child in rootNode)
                {
                    var key = child.Key;
                    if (((YamlScalarNode)key).Value == "GameObject")
                    {
                        _gameObjects.Add(new UnityGameObject(rootNode.Anchor, (YamlMappingNode)child.Value));
                    }
                }
            }
        }
    }
}
