# SnapshotIt
🎉 Snapshot It 1.0.0. Snapshot before change it, to make each object more secure to recovery.

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
var product = new Product()
{
    Id = 1,
    Name = "Nike"
}
// Let's Snapshot it 😜
Snap<Product>.Pick(product);

// |... Some logic ...| //

// You changed name of `Product`
product.Name = "Gucci";

// Now in program I want to get the previous state of product, and here we go !
product = Snap<Product>.ClapOne();
log.Information($"The previous product's name was {product.Name}");
```

