# AGENTS.md

## Build/Test Commands
No build or test commands - this is a Backstage templates repository (YAML only).
Test by pointing a Backstage instance at your branch (update app-config target URL).

## Code Style Guidelines

### File Structure
- Templates: `templates/scaffolder-templates/<name>/template.yaml` with `skeleton/` subdirectory
- Auto-discovered via glob in `showcase-templates.yaml` - no manual registration needed

### Template Conventions (template.yaml)
- Use Backstage Template spec `apiVersion: scaffolder.backstage.io/v1beta3`
- Service names: pattern `'^[a-z0-9-]+$'` (lowercase, hyphens only)
- Owner picker: `ui:field: MyGroupsPicker` (kubriX custom picker)
- RepoUrlPicker: set `allowedHosts`, `allowedOwners`, and `requestUserCredentials.secretsKey: USER_OAUTH_TOKEN`
- Publish step: `token: ${{ secrets.USER_OAUTH_TOKEN }}`, `repoVisibility: private` (default)

### Skeleton Files (Nunjucks)
- Variable injection: `${{values.parameterName}}` (no spaces around braces)
- Extract repo info: `${{ (parameters.repoUrl | parseRepoUrl)["owner"] }}`
- Required: `catalog-info.yaml` with `github.com/project-slug: owner/repo` annotation

### Standard Steps
1. `fetch:template` (render skeleton) → 2. `publish:github` (create repo) → 3. `catalog:register` (path: `/catalog-info.yaml`)
