# Copilot Instructions for kubrix-test-template

## Project Overview

This is a **Backstage Software Templates** repository for the kubriX platform. It contains scaffolder templates that developers use to bootstrap new services through the Backstage UI. The repository is registered as a catalog location in Backstage, not deployed as a service itself.

## Architecture & Structure

```
showcase-templates.yaml          # Location file: glob that discovers all templates
templates/scaffolder-templates/  # Template definitions
  <template-name>/
    template.yaml                # Backstage Template spec (v1beta3)
    skeleton/                    # Files to render and publish to new repos
```

**Key concept**: `showcase-templates.yaml` uses a glob pattern (`./templates/scaffolder-templates/**/template.yaml`) to auto-discover templates. New templates are automatically included when added to the correct path.

## Backstage Template Structure

Each `template.yaml` follows this pattern:

1. **metadata**: name, title, description, tags for discoverability
2. **spec.parameters**: UI form fields (uses JSON Schema + custom `ui:field` components)
3. **spec.steps**: actions to execute (fetch:template, publish:github, catalog:register)
4. **spec.output**: links shown to user after completion

**Templating syntax**: Skeleton files use `${{values.parameterName}}` to inject user inputs.

## Critical Conventions

### Repository Publishing
- Always set `allowedHosts` and `allowedOwners` in `RepoUrlPicker` to restrict where repos can be created
- Use `requestUserCredentials.secretsKey: USER_OAUTH_TOKEN` for GitHub publishing with user credentials
- Set `repoVisibility: private` by default for security

### Template Parameters
- Service names must match pattern `'^[a-z0-9-]+$'` (lowercase, URL-friendly)
- Use `ui:field: MyGroupsPicker` for owner selection (kubriX-specific custom picker)
- Extract repo owner/name using Nunjucks filters: `${{ (parameters.repoUrl | parseRepoUrl)["owner"] }}`

### Catalog Integration
- Generated repos must include `catalog-info.yaml` at root
- Use `github.com/project-slug` annotation format: `owner/repo`
- Always register the new component via `catalog:register` step with path `/catalog-info.yaml`

## Development Workflow

**Adding a new template:**
1. Create `templates/scaffolder-templates/<template-name>/template.yaml`
2. Add skeleton files in `templates/scaffolder-templates/<template-name>/skeleton/`
3. Commit and push - the glob in `showcase-templates.yaml` auto-discovers it
4. Backstage will pick it up when it refreshes catalog locations

**Testing templates:**
- No local scaffolder testing - changes must be tested in a Backstage instance
- Point Backstage at your branch: update the target URL in app-config to use your branch ref

**Common skeleton files:**
- `catalog-info.yaml`: Backstage component metadata (required)
- `README.md`: Service documentation template
- `.github/workflows/`: CI/CD if applicable

## Integration Points

- **Backstage catalog**: Templates register via `showcase-templates.yaml` Location kind
- **GitHub**: Templates publish repos using `publish:github` action with OAuth tokens
- **kubriX conventions**: Uses custom UI pickers (`MyGroupsPicker`) and assumes `group:platform-engineering` ownership model
