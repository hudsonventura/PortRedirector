# PortRedirector

It's create a way to redirect some ports to diferent destinies and ports. It works like a proxy, but not only HTTP conection, but any other type.    

To put it online, edit the file `appsettigs.json` and add all of your services as much as you need.  

``` json
{
    "redirects": [
        {
            "title": "test 1",          //it's just a name to show in log
            "port": 12345,              //it's the port you will conect
            "destiny": "10.10.1.201:22" //it's the url and port (respect the format 'url:port') that your connection is goint to bem redirected
        },
        {
            "title": "test 2",
            "port": 12344,
            "destiny": "10.10.1.201:24"
        }
    ]
}
```  
  

Don't forget to register the ports to be open on `docker-compose.yml`:
``` yaml
services:

  portredirector:
    #image: ${DOCKER_REGISTRY-}api
    container_name: portredirector
    build:
      context: ./src
      dockerfile: ./Dockerfile
    volumes:
      - "./appsettings.json:/app/appsettings.json"
    ports:
      - "12345-12350:12345-12350" # HERE!!!
``` 
  
  
After that, just up the containers:
``` bash
sudo docker compose up
```  
  
    
and, go ahead:
``` bash
test 1 -> TCP redirector started on 0.0.0.0:12345, redirecting to 10.10.1.201:22
test 2 -> TCP redirector started on 0.0.0.0:12344, redirecting to 10.10.1.201:24
```  

