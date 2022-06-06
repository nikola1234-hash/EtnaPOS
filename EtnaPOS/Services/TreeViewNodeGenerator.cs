using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EtnaPOS.Services
{
    public class TreeViewNodeGenerator : ITreeViewNodeGenerator
    {
        private EtnaDbContext _db => App.GetService<EtnaDbContext>();

        public ObservableCollection<Node> GenerateNodes()
        {
            ObservableCollection<Node> nodes = new ObservableCollection<Node>();
            var kategorije = _db.Kategorije;
            var artikli = _db.Artikli;
            foreach (var k in kategorije)
            {
                if(k.ParentId == 0)
                {
                    nodes.Add(new Node(k.Id, k.Kategorija, TypeNode.FolderClosed));
                }
                else
                {
                    var t = Collect(nodes).FirstOrDefault(s => s.Id == k.ParentId);
                    if(t != null)
                    {
                        t.Children.Add(new Node(k.Id, k.Kategorija, TypeNode.FolderClosed));
                    }
                    
                }
                
            }
            foreach (var node in Collect(nodes))
            {
                var c = artikli.Where(s => s.KategorijaArtiklaId == node.Id).ToList();

                foreach (var k in c)
                {
                    var type = k.IsActive == true ? TypeNode.Active : TypeNode.NotActive;
                    node.Children.Add(new Node(k.Id, k.Name, type));
                }
            }
            return nodes;
        }
        private IEnumerable<Node> Collect(IEnumerable<Node> nodes)
        {
            
            foreach (var node in nodes)
            {
                yield return node;
                foreach (var child in Collect(node.Children))
                    yield return child;
            }
                
        }
    }
}
