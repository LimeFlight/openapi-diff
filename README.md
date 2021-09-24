<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]


<!-- PROJECT LOGO -->
<h1>OpenAPI-Diff</h1>

<p>
  Compares two OpenAPI documents! 
  <br />
  <a href="https://github.com/LimeFlight/openapi-diff/issues">Report Bug</a>
  ·
  <a href="https://github.com/LimeFlight/openapi-diff/issues">Request Feature</a>
</p>
<br />


<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project


In order to identify any breaking API change of our B2B SaaS solution we created this project to outline the changes in the OpenAPI document.
We are using this together with GitHub Actions ([https://github.com/LimeFlight/openapi-diff-action](https://github.com/LimeFlight/openapi-diff-action)) to highlight changes in every Pull Request.


<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple example steps.

### Prerequisites

Coming soon.

### Installation

Coming soon.



<!-- USAGE EXAMPLES -->
## Usage

Coming soon.

### CLI

Install globally:

```shell
dotnet tool install --global LimeFlight.OpenAPI.Diff.CLI --version 3.0.9
```

Invoke:

```
openapi-diff --help
```
```
Options:
  -o|--old <OLD_PATH>       Path to old OpenAPI Specification file
  -n|--new <NEW_PATH>       Path to new OpenAPI Specification file
  -e|--exit <EXIT_TYPE>     Define exit behavior. Default: Fail only if API changes broke backward compatibility
                            Allowed values are: PrintState, FailOnChanged.
  -m|--markdown <MARKDOWN>  Export diff as markdown in given file
  -c|--console              Export diff in console
  -h|--html <HTML>          Export diff as html in given file
  -?|--help                 Show help information.
```

**Example:**

```shell
directory="test/LimeFlight.OpenApi.Diff.Test/Resources";
openapi-diff -o $directory/petstore_v2_1.yaml -n $directory/security_diff_1.yaml -m difflog.md
```

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.



<!-- CONTACT -->
## Contact

LimeFlight - hello@limeflight.com

Project Link: [https://github.com/LimeFlight/openapi-diff](https://github.com/LimeFlight/openapi-diff)



<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements
* [OpenAPITools OpenAPI-diff](https://github.com/OpenAPITools/openapi-diff)
* [Img Shields](https://shields.io)
* [Choose an Open Source License](https://choosealicense.com)




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/LimeFlight/openapi-diff?style=flat-square
[contributors-url]: https://github.com/LimeFlight/openapi-diff/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/LimeFlight/openapi-diff?style=flat-square
[forks-url]: https://github.com/LimeFlight/openapi-diff/network/members
[stars-shield]: https://img.shields.io/github/stars/LimeFlight/openapi-diff?style=flat-square
[stars-url]: https://github.com/LimeFlight/openapi-diff/stargazers
[issues-shield]: https://img.shields.io/github/issues/LimeFlight/openapi-diff?style=flat-square
[issues-url]: https://github.com/LimeFlight/openapi-diff/issues
[license-shield]: https://img.shields.io/github/license/LimeFlight/openapi-diff?style=flat-square
[license-url]: https://github.com/LimeFlight/openapi-diff/blob/master/LICENSE.txt
[product-screenshot]: images/screenshot.png
