# Docker for windows

install https://docs.docker.com/docker-for-windows/install/#start-docker-for-windows
(automatically enables hyper-v and restarts machine)

this installs `docker, docker-machine, docker-compose`

Start docker manually

Check version
```cmd
> docker --version
```

Run nginx webserver on localhost:8080
```bash
> docker run -d -p 8080:80 --name webserver nginx
> docker stop webserver
> docker start webserver
```
Open browser to address to test it all went well

Setup tab completion in powershell
1. Start powershell as admin
2. `> Set-ExecutionPolicy RemoteSigned`
3. `> Install-Module posh-docker`
4. `> Import-Module posh-docker`

To persist tab completion across sessions
1. Open powershell profile `> code $profile` (or `Notepad` if vs code is not in path)
2. Add `Import-Module posh-docker` to end of file

> If you use powershell via Cmder you might have multiple profile files, just search for `profile.ps1` to find where the one you want is located.

### Kitematic
Graphical user interface to manage containers.
Right click docker icon in system tray, select `Kitematic`
Download zip file by clicking the link, extract it and copy contents to the path displayed in the popup (`C:\Program Files\Docker\Kitematic`)

Test kitematics `hello-world-nginx`, to get it working i first deleted the container already there and added it again from the user interface (+). At that time I got questions about sharing my drive with docker etc that made volumes mounting work.

**Proxying tv.dev.nrk.no from this image**
First make sure that tv.dev.nrk.no is set up against your public ip in hosts file.
For me: 160.67.52.40.
If you are using 127.0.0.1, nginx will resolve this and then get Bad Gateway since 127.0.0.1 will no linger be the local host of the host machine but of the container itself.

`> vi etc/nginx/nginx.conf`

```
    location / {
        proxy_pass http://tv.dev.nrk.no/;
        add_header X-Proxy-Magic nginx-docker;
    }
```


## Try try and test

### Simple NodeJs from dockerfile

Dockerfile
```dockerfile
FROM mhart/alpine-node

WORKDIR /app
COPY . .

# If you have native dependencies, you'll need extra tools
# RUN apk add --no-cache make gcc g++ python

RUN npm install --production

# Define environment variable
ENV PORT 3000

# Make port available to the world outside this container
EXPOSE 3000

CMD ["npm", "start"]
```

index.js
```js
module.exports = (/** @type {IncomingMessage} */request) => {
  const { url, headers, method } = request;

  return { msg: 'Hello from Node/Micro',
    req: { url },
    env: process.env
  }
};
```

package.json
```json
{
  "name": "node-one",
  "version": "1.0.0",
  "main": "index.js",
  "scripts": {
    "start": "micro -p $PORT"
  },
  "dependencies": {
    "micro": "^9.1.0"
  }
}
```

Build the dockerfile and tag the image with a name
```
$ docker build -t node-one .
```
List images to see that there is one called "node-one"
```
$ docker images
```

Run with kitematic by creating new container from  "My Images", or execute in shell
```
# docker run [OPTIONS] image[:TAG|@DIGEST] [COMMAND] [ARG...]

$ docker run -d -t -P --name MyNodeOne node-one

# '-d': run in detached mode (run in background)
# '-t': allocate a pseudo-TTY session (emulated terminal)
# '-P': assign random external port to exposed internal port (3000)
# '--name': set name to MyNodeOne
```

[Docker cheat sheet](https://github.com/wsargent/docker-cheat-sheet)
[Docker CLI reference](https://docs.docker.com/engine/reference/commandline/docker/)

### Simple NodeJs with docker-compose

Add a `docker-compose.yml` file containing

```yml
version: '3'
services:
  web:
    build: .
    ports:
     - "5000:3000"
```

Run the thingy with `docker-compose up` in the current directory

If you change the code, run `docker-compose build` before running `docker-compose up` again.

For more details see [Getting started with docker-compose](https://docs.docker.com/compose/gettingstarted/)

> I tried to run the app with nodemon and setting `volumes: - .:/app` (in the compose file) to be able to make local changes to code but unfortunately it seems that the file events does not work across windows/docker. Changes made with `vi` in the container shell worked though.


To do a more thourough cleanup of stopped containers and unused images on your system run `docker system prune -a`. For more details and tips, see:
[How to remove images, containers and volumes](https://www.digitalocean.com/community/tutorials/how-to-remove-docker-images-containers-and-volumes)

### Simple NodeJs and nginx with docker-compose

[See nginx-examples on GitHub](https://github.com/tolu/nginx-examples)
