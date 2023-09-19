# SnapshotIt
🎉 Snapshot It 1.0.0. Before changing the state of component, just snapshot the state of object to make it easy to recovery.

![image](https://github.com/AkhmedovEhson/SnapshotIt/blob/main/assets/iconforgithub.png)

# Requirements
* .NET 7

# Documentation
`Snapshot It` is mini-library(utils) to make coding more easy. There are some moments when you update state of class and can not to recovery the previous state, here comes `Snapshot It`.

🖊️ We have a class `Product` (?)
```
public class Product
{
    public int Id {get;set}
    public string Name{get;set;}
}
```
🐹 Let's do following steps....
```
var product = new Snap<Product>(new Product()
{
    Id = 1,
    Name = "Nike"
}).ClapOne();

product.Name = "Gucci";

// Note: Get previous state of component
product = Snap<Product>.ClapOne();
log.Information($"The previous product's name was {product.Name}"); 
```
Returns:
- The previous product's name was Nike

