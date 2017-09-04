## POLYMER SUMMIT 2017

### rendertron - SSR for SEO with Google Headless
RenderTron - SSR dockerized service/app for web components
https://github.com/googlechrome/rendertron

<npm> prpl-server - node server for production PRPL apps (H2/H2Push)

rendering SSR as AzureFunction / FireBase Function / AWS Lambda ?
Caching inside a server less function?

### VirtualReality

Polymer based D3 setup.

Scene graph provided as DOM tree!!!

Vanilla WebVR

```js
navigator.getVRDisplays().then((displays) => {...})

// only triggered on user gesture
function onVREnter () { 
vrDisplay.requestPresent({ source: canvasElem });
}

function render(){
    // get native refresh rate - ups the animationFrame to be faster than 60fps if possible
    vrDisplay.getFramerate(frameData);
    // render for each eye
    vrDisplay.submitFrame();
    // loop
    vrDisplay.requestAnimationFrame(render);
}
```

#### Extending components for WebVR
```html
<three-scene vr="auto">
    <!-- place camera inside vr-controls!!! -->
    <three-vr-controls type="lookaround">
        <three-camera></three-camera>
    </three-vr-controls>
    
    <three-mesh position="0 0 -5">
        <three-geometry type="box"></three-geometry>
        <three-material type="baseic" color="0x00FF00"></three-material>
    </three-mesh>
</three-scene>
```

You wont find this lib on the internet...
Use Mozilla A-Frame (came out 2015)

### What's next for Polymer

3 big changes coming to polymer

* joining npm eco system (leaving bower)
* embracing ES-modules
* auto-upgrade tool for v2 -> v3

**NPM**  
bower deprecated since some time  
yarn resolves issues that npm has regarding version conflicts and flat dependency tree's

```json
// package.json
{
    "flat": true, // <-- yarn
    "dependencies": {
        "@polymer/polymer": "^3.0.0"
    }
}
```

**ES-modules**  
ES-modules then replacing HTML Imports (which was really just a thing that came about while waiting for a better loading system for js/html etc)

HTML Imports also ever only supported by Chrome / Opera

ES-modules available in Safari, shipping sept 5 in Chrome/Opera and in development for Edge / Firefox

i.e. no polyfills needed for Chrome/Safari/Opera to use polymer / web components

**Example**

```js
// importing
import {Element} from '../@polymer/polymer/polymer-element.js'

// move your template into class definition
class PrettyButton extends Element {
    static get properties() {}
    static get observers() {}
    static get templates() {
        return `
            <style> /* css */ </style>
            <slot> /* children */ </slot>
        `;
    }
}
// export
export PrettyButton;
```

All 96 polymer elements are available on npm today (preview).

```bash
> yarn add @polymer/polymer@next
> yarn add @polymer/paper-button@next
```

### Polymer @ Netflix (cloud platform egineering)

Works on Cassandra, Dynamite, Elastic, Kafka, Atlas etc

All these need self service apps, dashboards etc

Lumen - Netflix internal dashboard builder (avilable for others?)

Use redux by itself in `tv.nrk.no`?
How would it be to replace `nrk.state` with a redux store?

Check polycast #61/62 and polymer-redux on github.

### Polymer in Ionic

Ionic: bootstrap for mobile (built with typescript)  
"Native" elements for web

PWA in Ionic 2.0 was not cutting it (1mb browser download, 13s 3G interactive etc)

Announcing Stencil - a compiler for web components
Powering the next version of Ionic  
http://stenciljs.com

Supports TypeScript, SSR, Pre-compilation, Async-rendering, reactive data bindings, dev server

### Alex Russel - WC in the nick of time

chromeframe for IE - what is that?

work on web components started in 2010...

PWA polymer shop app - check it out!

### UI as enterprise (EA) at scale

use fuzzy version matching on static.nrk.no?

like:
* static.nrk.no/core-css/1/core-css.min.css
* static.nrk.no/core-css/1.30/core-css.min.css
* static.nrk.no/core-css/1.31.2/core-css.min.css

should all resolve to the same version.
fuzzy version mask redirects to heavily cached absolutely versioned file (the last in list)
Links that trigger redirects are cached for 1h.

If we had more components
* normal web
* mobile web
* desktop apps (UWP, Electron etc)
* smart tv's with ok browser

Components handle personalisation for its children.
Components handle ga analytics

### End to End apps with polymer

Router logic should be in app-shell bundle

SW cache logic: static files 
* tv.nrk.no/content
* tv.nrk.no/scripts
* tv.nrk.no/pakke73/

Next we need to differentiate on data and normal html routes on our own domain...use accepts header?
Can we know if a request is the initial request (navigation request)
* tv.nrk.no/serie/skam => html
* tv.nrk.no/userdata => json

On top of that we need to cache certain app shell files, like external dependencies - styleguide, small image versions?

cache busting sw cache? Pope?

#### Mediator pattern

mediator component is the parent scope of other components that need shared state.

Responsible to propagate data down and listening for events that bubble up, massage data and propagate down again.

The mediator is a data/events only component. E.g. flux.
Can have global mediator...(called store in redux)

Recommends redux. 

State passed to elements via 'subscribe' callback
Actions dispatched to store by elements
Reducers return new state after actions

#### Connecting elements to redux

Create generic element with properties/events
Subclass generic element for all other elements ('connected element')

Ensure redux logic is lazily loaded!!!

```js
import {hmm} from 'foobar';

store.addReducers({whateverNeededInsideComponent});

class LoginButton extends ConnectedElement {
    constructor() {
        super();
        // set state to properties
        store.subscribe( state => {
            this.setProperties({
                selected: state.loggedIn
                name: state.userName
            })
        })
        // dispatch actions based on events
        this.addEventListener('user-login', e => {
            store.dispatch(e.detail)
        })
    }
}
```

Checkout `polymer-redux` library...

#### `prpl-server-node`
* serve non transpiled code for modern browsers
* use H2 Push when possible
* serve bundled ES6 if no H2
* serve ES5 bundled for older browsers
* bots => serve rendered html

### Rob Dodson

"how to component"

check out https://bit.ly/building-components

https://custom-elements-everywhere.com

### SuperCharged live

Custom element example with intersectionObserver n stuff

```js
// create template outside of element to avoid triggering the parser - faster when creating many elements
const template = document.createElement('template');
template.innerHTML = `
    <style>
        :host {
            display: block;
        }
        img {
            width: 100%;
        }
    </style>
`;

// wait to load image until it comes into view

const io = new IntersectionObserver(entries => {
    for(const entry of entries){
        if(entry.isIntersecting) {
            entry.target.setAttribute('full', '')
        }
    }
});


class SCImage extends HTMLElement {
    static get observedAttributes() {
        return ['full']
    }
    constructor(){
        super();
        this.attachShadow({mode: 'open'});
        this.shadowRoot.appendChild(template.content.cloneNode(true));
    }
    
    connectedCallback() {
        io.observe(this);
    }
    disconnectedCallback() {
        io.unobserve(this);
    }
    
    get full() {
        return this.hasAttribute('full')
    }
    get src() {
        return this.getAttribute('src');
    }
    attributeChangedCallback(){
        if(this.loaded){
            return;
        }
        const img = document.createElement('img');
        img.src = this.src;
        img.onload = () => {
            this.loaded = true;
            this.shadowRoot.appendChild(img);
        }
        
    }
}

customElements.define('sc-image', SCImage);
```

```html
    <sc-img src="some/image/path" />
```

### Crockford - The Post JavaScript Apocalypse

Talks about his RQ lib for promises++ - check it out!

Be functionally pure as often as you can (that means not using class?)

