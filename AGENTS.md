# AGENTS.md

## Build/Test Commands
No build or test commands - this is a Backstage templates repository (YAML only).
Test templates by pointing a Backstage instance at your branch.

## Code Style Guidelines

### File Structure
- Templates live in `templates/scaffolder-templates/<name>/template.yaml`
- Skeleton files go in `skeleton/` subdirectory
- Templates auto-discovered via glob in `showcase-templates.yaml`

### Template Conventions (template.yaml)
- Use Backstage Template spec `v1beta3`
- Service names: pattern `'^[a-z0-9-]+$'` (lowercase, hyphens allowed)
- Owner picker: `ui:field: MyGroupsPicker`
- Repo picker: set `allowedHosts` and `allowedOwners` in RepoUrlPicker (NOT in publish step)
- Token: `token: ${{ secrets.USER_OAUTH_TOKEN }}` in publish step
- Visibility: `repoVisibility: internal` (or private/public as needed)

### Skeleton Files (Nunjucks templating)
- Use `${{values.parameterName}}` syntax for variable injection
- Always include `catalog-info.yaml` with `github.com/project-slug` annotation
- Extract repo info: `${{ (parameters.repoUrl | parseRepoUrl)["owner"] }}`

### Standard Steps Sequence
1. `fetch:template` - render skeleton  2. `publish:github` - create repo  3. `catalog:register` - register component
