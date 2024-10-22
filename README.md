### **Kind of Bread**
A project for organising grain-based foodstuffs into different kinds of bread.

### Database setup
To setup a database to work with the application make sure to have a MySQL-image in Docker or, in a terminal of your choice, create one through: 
>docker pull mysql:latest

Then create a container using:
>docker run --name _name-of-the-container_ -p port:port -e MYSQL_ROOT_PASSWORD=_password_ -d mysql

Where _name-of-the-container_ is the name you want to give to your container, _port_ is the host port and container port (normally 3306:3306 for MySQL) and _password_ is the password you want to use. 

Then run:
>docker exec -it _name-of-the-container_ bash

Then run: 
>mysql -h _host_ -P _port_ -u root -p

And you will be prompted to enter your password. 

If you choose to omit -h _host_ and -P _port_ it will, in windows, default to _localhost_ and _3306_.

You can create your database through: 
>CREATE DATABASE _name-of-your-database_;

Then use it:
>USE _name-of-your-database_;

Now you can construct the tables:

**bread_type**
```
CREATE TABLE bread_type (
    id INT AUTO_INCREMENT PRIMARY KEY,
    type VARCHAR(255)
);
```

**bread_item**
```
CREATE TABLE bread_item (
    id INT AUTO_INCREMENT PRIMARY KEY,
    type_id INT,
    name VARCHAR(255),
    grain VARCHAR(255),
    fermented BOOLEAN,
    viscosity VARCHAR(255),
    texture VARCHAR(255),
    shape VARCHAR(255),
    dressed BOOLEAN,
    FOREIGN KEY (type_id) REFERENCES bread_type(id)
);
```

**bread_addon**
```
CREATE TABLE bread_addon (
    id INT AUTO_INCREMENT PRIMARY KEY,
    bread_id INT,
    addon VARCHAR(255),
    type VARCHAR(255),
    FOREIGN KEY (bread_id) REFERENCES bread_item(id)
);
```

Then change defaultconnection in appsettings.json in the backend project to use your settings.
>"Server=_host_;Database=_name_;Uid=_user_;Pwd=_password_;"

Verify that the host variable in the frontend bread service is the same as your backend port. 