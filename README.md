# kubrix-test-template

Backstage software templates for kubriX product units. Point your Backstage instance at this repository to offer the contained templates to your developers.

## How to register in Backstage

Add the location file to your Backstage catalog locations (for kubriX, put this in `backstage-resources/entities` or your app-config):

```yaml
- type: url
  target: https://github.com/mjay-bs-test/kubrix-test-template/main/showcase-templates.yaml
```

If the repo is private, configure Backstage with a token that can read it.

## Templates

- `basic-service-repo`: creates a starter service repository with a Backstage `catalog-info.yaml`, README, and publishes to GitHub.

## Developing new templates

1. Add a folder under `templates/scaffolder-templates/<your-template>/`.
2. Put your `template.yaml` there plus any skeleton files the template should render.
3. The glob in `showcase-templates.yaml` will auto-include it. Commit and push.

Tip: When publishing repos, ensure the template restricts `allowedHosts/allowedOwners` in `RepoUrlPicker` and that you have a `USER_OAUTH_TOKEN` secret configured in Backstage for GitHub access.
