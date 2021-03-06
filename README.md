# Engineer's Notebook

![Dependency Graph](/docs/graph.PNG)

The design follows the `Clean Architecture` paradigm. Ultimately, the `Public API` is a microservice which helps us manage and retrieve Wiki-based documentation.

The `Blazor Admin` project is a front-end tailored for administrative duties of managing documentation records. It comes with a WYSIWYG editor. Independent service - not required in prod, but does simplify management. 

`Skynet` project will use the `Public API` to help automate the answering of questions users have on our Discord server.

## Screenshots
![docs view](/docs/docs_view.png)

![view only](/docs/view_only.png)

![edit view](/docs/edit_view.png)

![new doc](/docs/new_doc.png)

![tags](/docs/tags.png)
