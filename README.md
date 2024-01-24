# SnapshotIt
🎉 Snapshot It 1.0.0. Before changing the state of component, just snapshot the state of object to make it easy to recovery.

![image](https://github.com/AkhmedovEhson/SnapshotIt/blob/main/assets/iconforgithub.png)

# Requirements
* .NET 6 🔒️ .NET 7 

# Documentation
💚 `Snapshot It` is extensionable lightweight-library, provides bunch of extensions.
<ul>
    <li>📝 Snapshotting - simply snapshot any instance in any context of application, and use it in runtime of application.</li>
    <li>🗃️ Copying - copy any instance.</li>
    <li>🌐 Channel `BufferBlock<T>` - use it as unbounded channel, post, read, close.</li>
</ul>

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
    Id = 1
    Name = "Nike"
}
Snapshot.Out.Post(product); // captures state
product.Id = 2;
product.Name = "Gucci";

const int index = 0;
// Gets the first snapshot from capture collection
// By default index = 0, using index easily can find the correct captured instance
product = Snapshot.Out.Get(index);


// Logs: The product's name is Gucci
log.Information($"The product's name is {product.Name}");

// Logs: The previous product's name was Nike 
log.Information($"The previous product's name was {product.Name}");
```

