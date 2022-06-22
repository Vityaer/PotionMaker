using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StructsData{
	[System.Serializable]
	public class Node<T>{
		public T data;
		public Node<T> next{get; set;}
		public Node(T data){
			this.data = data;
		}
	}
	[System.Serializable]
	public class CircList<T> : IEnumerable<T>{
		Node<T> head;
		Node<T> tail;
		int count;
		public void Add(T data){
			Node<T> newNode = new Node<T>(data);
			if(head != null){
				newNode.next = head;
				tail.next = newNode;
				tail = newNode;
			}else{
				head = newNode;
				tail = newNode;
				tail.next = head;
			}
			count++;
		}
		public bool Remove(T data){
			Node<T> previous, current;
			if(head == null) return false;
			previous = tail;
			current = head;
			int k = 0;
			while((!current.data.Equals(data)) && (k < count)){
				previous = current; 
				current  = previous.next;
				k++;
			}
			if(k != count){
				if(count > 1){
				previous.next = current.next;
				}else if(count == 1){
					head = tail = null;
				}
				count--;
				return true;
			}else{
				return false;
			}	
		}
		public int Count{ get => count;}
		public void Clear(){
			head = tail = null;
			count = 0;
		} 
		
		public bool Contains(T data){
            Node<T> current = head;
            if (current == null) return false;
            do{
                if (current.data.Equals(data))
                    return true;
                current = current.next;
            }
            while (current != head);
            return false;
        }
         IEnumerator IEnumerable.GetEnumerator(){
            return ((IEnumerable)this).GetEnumerator();
        }
 
        IEnumerator<T> IEnumerable<T>.GetEnumerator(){
            Node<T> current = head;
            do{
                if (current != null){
                    yield return current.data;
                    current = current.next;
                }
            }
            while (current != head);
        }
    }
}