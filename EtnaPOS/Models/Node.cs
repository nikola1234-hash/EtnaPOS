using System.Collections.ObjectModel;

namespace EtnaPOS.Models
{
    public enum TypeNode
    {
        Active,
        NotActive,
        FolderOpen,
        FolderClosed
    }
    
    public class Node
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public TypeNode Type { get; set; } 
        public ObservableCollection<Node> Children { get; set; }
        public Node(int id, string name, TypeNode type)
        {
            Id = id;
            Name = name;
            Type = type;
            Children = new ObservableCollection<Node>();
        }

    }
   

}
