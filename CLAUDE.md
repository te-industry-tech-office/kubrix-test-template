# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a **Backstage Software Templates** repository for kubriX. It contains scaffolder templates that bootstrap new services through the Backstage UI. The repository is registered as a catalog location in Backstage (not deployed as a service).

## Repository Structure

```
showcase-templates.yaml          # Location file: glob that discovers all templates
templates/scaffolder-templates/  # Template definitions
  <template-name>/
    template.yaml                # Backstage Template spec (v1beta3)
    skeleton/                    # Files rendered and published to new repos
```

**Key architecture**: `showcase-templates.yaml` uses a glob pattern (`./templates/scaffolder-templates/**/template.yaml`) to auto-discover templates. New templates are automatically included when added to the correct path structure.

## Backstage Template Architecture

Each `template.yaml` follows the Backstage Template v1beta3 spec:

1. **metadata**: name, title, description, tags for UI discoverability
2. **spec.parameters**: UI form fields using JSON Schema + custom `ui:field` components
3. **spec.steps**: scaffolder actions to execute (`fetch:template`, `publish:github`, `catalog:register`)
4. **spec.output**: links shown to user after template execution completes

**Templating syntax**: Skeleton files use Nunjucks templating with `${{values.parameterName}}` to inject user inputs. Nunjucks filters are available, e.g., `${{ (parameters.repoUrl | parseRepoUrl)["owner"] }}`.

## Critical Template Conventions

### Repository Publishing
- Always set `allowedHosts` and `allowedOwners` in `RepoUrlPicker` UI field (in parameters section) to restrict where repos can be created
- **IMPORTANT**: Do NOT include `allowedHosts` in the `publish:github` action input - it only belongs in the `RepoUrlPicker` configuration
- Use `requestUserCredentials.secretsKey: USER_OAUTH_TOKEN` for GitHub publishing with user credentials
- Set `repoVisibility: private` by default for security
- Include `deleteBranchOnMerge: true` for clean repo hygiene

### Template Parameters
- Service names must match pattern `'^[a-z0-9-]+$'` (lowercase, URL-friendly, hyphens allowed)
- Use `ui:field: MyGroupsPicker` for owner selection (kubriX-specific custom picker)
- Extract repo owner/name using Nunjucks filters: `${{ (parameters.repoUrl | parseRepoUrl)["owner"] }}`

### Catalog Integration
- Generated repos must include `catalog-info.yaml` at root
- Use `github.com/project-slug` annotation format: `owner/repo`
- Always register the new component via `catalog:register` step with `catalogInfoPath: '/catalog-info.yaml'`
- Default ownership structure: `spec.owner: group:platform-engineering` for templates, user-selected for generated components

## Development Workflow

### Adding a New Template
1. Create directory: `templates/scaffolder-templates/<template-name>/`
2. Add `template.yaml` with Backstage Template spec
3. Add skeleton files in `skeleton/` subdirectory
4. Commit and push - the glob in `showcase-templates.yaml` auto-discovers it
5. Backstage picks it up when it refreshes catalog locations

### Testing Templates
- No local scaffolder testing available
- Test in a Backstage instance by pointing catalog at your branch
- Update the target URL in Backstage app-config to use your branch ref:
  ```yaml
  - type: url
    target: https://raw.githubusercontent.com/te-industry-tech-office/kubrix-test-template/<branch-name>/showcase-templates.yaml
  ```

### Common Skeleton Files
- `catalog-info.yaml`: Backstage component metadata (required for catalog registration)
- `README.md`: Service documentation template
- `.github/workflows/`: CI/CD workflows if applicable
- Any starter code or configuration files

## Integration with kubriX IDP Platform

This template repository integrates with the **kubriX** Internal Developer Platform (IDP), a comprehensive Kubernetes-based platform that provides GitOps, observability, security, and developer self-service capabilities.

### How Templates Are Discovered

The kubriX Backstage instance registers this repository as a catalog location:

```yaml
catalog:
  locations:
    - type: url
      target: https://github.com/te-industry-tech-office/kubrix-test-template/blob/main/showcase-templates.yaml
      rules:
        - allow: [Template]
```

This configuration is defined in the kubrix-idp platform's Backstage values (e.g., `platform-apps/charts/backstage/values-*.yaml`).

### kubriX Platform Components

Templates in this repository can leverage the following kubriX platform services:

- **ArgoCD**: GitOps continuous delivery engine
- **Kargo**: Multi-stage GitOps promotion (dev → test → prod)
- **Keycloak**: Identity and Access Management (IAM)
- **Vault**: Secrets management
- **Grafana LGTM**: Observability (Loki, Grafana, Tempo, Mimir)
- **Kyverno**: Policy enforcement
- **KubeCost**: Cost visibility and management

### Multi-Stage Applications with Kargo

For multi-stage application templates (dev, test, prod environments), include these files in the skeleton:

**app-stages.yaml**: Defines the ArgoCD apps for each stage and Kargo promotion pipeline
```yaml
# Example structure (placed at repo root):
stages:
  - name: dev
  - name: test
  - name: prod
```

**catalog-info.yaml**: Should define umbrella component and stage-specific subcomponents
```yaml
apiVersion: backstage.io/v1alpha1
kind: Component
metadata:
  name: ${{values.name}}
  annotations:
    github.com/project-slug: ${{values.repoOwner}}/${{values.repoName}}
spec:
  type: service
  lifecycle: experimental
  owner: ${{values.owner}}
  subcomponentOf: # Optional: reference parent component
```

**Helm Chart Structure**:
- `Chart.yaml`: Helm chart metadata
- `values.yaml`: Default values (use `{{ .Values.kubriXIngressDomain }}` for ingress domain)
- `values-dev.yaml`: Dev-specific overrides
- `values-test.yaml`: Test-specific overrides
- `values-prod.yaml`: Prod-specific overrides
- `templates/`: Kubernetes manifests

### ArgoCD ApplicationSet Integration

The kubriX platform uses ArgoCD ApplicationSets to automatically discover and deploy applications. Templates should follow naming conventions:

- Repository name pattern: `<team-name>-<app-name>` (e.g., `team-a-my-service`)
- Include `app-stages.yaml` at repository root for multi-stage apps
- ApplicationSets use SCM generators to scan GitHub organizations for repos matching these patterns

### Team Onboarding

Before creating application templates, teams must be onboarded via the platform's team-onboarding template, which:

1. Creates an ArgoCD project for the team with restricted permissions
2. Sets up team-specific "App-of-Apps" repository
3. Configures ArgoCD ApplicationSets to discover team applications
4. Establishes namespace boundaries (e.g., `team-a-*` namespaces)

### Required GitHub Tokens

Templates that publish to GitHub require tokens stored in Backstage:

- **USER_OAUTH_TOKEN**: User's OAuth token for repository creation (scopes: `repo`, `workflow`)
- **KUBRIX_ARGOCD_APPSET_TOKEN**: Read access for ArgoCD ApplicationSet SCM discovery
- **KUBRIX_KARGO_GIT_PASSWORD**: Write access for Kargo GitOps promotion

These tokens are managed through Vault and injected into Backstage via External Secrets Operator.

### Authentication & Authorization

- **GitHub OAuth**: User authentication via GitHub (configured in Backstage)
- **Keycloak OIDC**: Platform-wide SSO integration
- **RBAC**: Role-based access control enforced via Backstage permissions and ArgoCD projects
- **Group Membership**: `MyGroupsPicker` UI field pulls groups from Keycloak

## Integration Points

- **Backstage catalog**: Templates register via `showcase-templates.yaml` Location kind
- **GitHub**: Templates publish repos using `publish:github` action with OAuth tokens
- **ArgoCD**: Generated repos automatically discovered by ApplicationSets based on naming conventions
- **Kargo**: Multi-stage promotion configured via `app-stages.yaml`
- **Vault**: Secrets (tokens, credentials) injected into Backstage at runtime
- **kubriX conventions**: Uses custom UI pickers (`MyGroupsPicker`) and assumes `group:platform-engineering` ownership model for platform resources

## Backstage Template Steps Reference

Standard action sequence for service templates:
1. `fetch:template` - renders skeleton files with user parameters
2. `publish:github` - creates GitHub repository and pushes rendered content
3. `catalog:register` - registers the new component in Backstage catalog

Advanced multi-stage template sequence:
1. `fetch:template` - renders Helm chart, app-stages.yaml, catalog-info.yaml
2. `publish:github` - creates repository with multi-stage structure
3. `catalog:register` - registers umbrella component (subcomponents registered via app-stages.yaml)

Access outputs from previous steps using: `${{ steps.<step-id>.output.<property> }}`

## Platform Architecture Reference

**GitOps Workflow**:
1. Developer creates app via Backstage template → GitHub repo created
2. ArgoCD ApplicationSet discovers repo via SCM generator → ArgoCD apps created
3. ArgoCD syncs Helm chart to dev cluster
4. Kargo promotes changes: dev → test → prod (manual approval gates)
5. Backstage provides unified view of all stages and health status

**Repository Locations**:
- Template repository: `github.com/te-industry-tech-office/kubrix-test-template`
- Platform configuration: `../kubrix-idp` (relative path from this repo)
- Team application repos: Created dynamically via templates
- Team "App-of-Apps" repos: Created via team-onboarding template
