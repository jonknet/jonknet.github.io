using System.Collections.Generic;

public class TreeNode<T> {
    
    public TreeNode<T> ParentNode { get; set; }
    public IList<TreeNode<T>> NextNodes { get; set; }
    public T Content { get; set; }

    public TreeNode(T content){
        Content = content;
        ParentNode = null;
        NextNodes = new List<TreeNode<T>>();
    }

    public TreeNode(T content, TreeNode<T> parent){
        Content = content;
        ParentNode = parent;
        NextNodes = new List<TreeNode<T>>();        
    }

    public void AddChildNode(T content){
        NextNodes.Add(new TreeNode<T>(content,this));
    }

    public void RemoveChildNode(T content){
        for(int i = 0; i < NextNodes.Count; i++){
            if(NextNodes[i].Content.Equals(content)){
                NextNodes.RemoveAt(i);
                break;
            }
        }
    }

    public TreeNode<T> GetRootNode(){
        TreeNode<T> current = this;
        while(current.ParentNode != null){
            current = current.ParentNode;
        }
        return current;
    }

    public int GetHeight(){
        int height = 1;
        TreeNode<T> current = this;
        while(current.ParentNode != null){
            height++;
            current = current.ParentNode;
        }
        return height;
    }

}