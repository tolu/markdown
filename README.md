# markdown
Just a collection of markdown files ranging from random notes to useful blabber.

## Markdown tricks
### Collapsible content with `<details>`

[`<details>` on MDN](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/details)

Example:  
```html
<details>
  <summary>This is the header</summary>
  And here is the expandable content!
</details>
```
And in action: 
<details>
  <summary>Summary header (click me!)</summary>
  > The HTML `<details>` element is used as a disclosure widget from which the user can retrieve additional information.
  
  ```js
  //code works perfectly
  export default () => {
    return (<h1>Hey!</h1>)
  }
  ```
#### And subheaders
  If they are not indented anyway...

  **open**  
  This Boolean attribute indicates whether the details will be shown to the user on page load.
  Default is false and so details will be hidden.
</details>

### Using your github avatar  
`![alt text for tobias avatar](https://avatars.githubusercontent.com/u/658586?s=150)`

![alt text for tobias avatar](https://avatars.githubusercontent.com/u/658586?s=150)

### repos to try out
* [morpdom](https://github.com/patrick-steele-idem/morphdom)
* [inferno](https://github.com/trueadm/inferno)
* [next.js](https://github.com/zeit/next.js)
* [gatsby](https://github.com/gatsbyjs/gatsby)
