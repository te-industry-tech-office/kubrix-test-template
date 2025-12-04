# ${{values.team}}-${{values.application_id}} Deployment

## Overview

This repository contains the GitOps deployment configuration for **${{values.application_id}}** owned by **${{values.team}}**.

It uses the kubriX SCM generator pattern with Kargo for progressive delivery across environments.

## Repository Structure

```
├── templates/            # Helm templates
│   ├── deployment.yaml
│   ├── service.yaml
│   └── ingress.yaml
├── app-stages.yaml       # Kargo stage definitions
├── values.yaml           # Base Helm values
├── values-dev.yaml       # Dev environment overrides
├── values-test.yaml      # Test environment overrides
├── values-prod.yaml      # Prod environment overrides
└── docs/                 # Documentation (this folder)
```

## Related Repositories

| Repository | Purpose |
|------------|---------|
| [${{values.team}}-${{values.application_id}}-dev](https://github.com/${{values.repoOwner}}/${{values.team}}-${{values.application_id}}-dev) | Application source code |
| [${{values.team}}-${{values.application_id}}-deploy](https://github.com/${{values.repoOwner}}/${{values.team}}-${{values.application_id}}-deploy) | GitOps deployment (this repo) |

## Environments

| Environment | URL | Cluster |
|-------------|-----|---------|
| Dev | http://${{values.team}}-${{values.application_id}}.dev.${{values.fqdn}} | dev001 |
| Test | http://${{values.team}}-${{values.application_id}}.test.${{values.fqdn}} | test001 |
| Prod | http://${{values.team}}-${{values.application_id}}.prod.${{values.fqdn}} | prod001 |

## Deployment Pipeline

1. **Dev**: Auto-deployed on new container image
2. **Test**: Promoted via Kargo after dev validation
3. **Prod**: Promoted via Kargo after test validation

## Links

- [ArgoCD Application](https://argocd.${{values.fqdn}}/applications/adn-${{values.team}}/${{values.team}}-${{values.application_id}})
- [Kargo Project](https://kargo.${{values.fqdn}}/project/${{values.team}}-${{values.application_id}}-kargo-project)
- [Grafana Dashboard](https://grafana.${{values.fqdn}}/d/k8s_views_ns/kubernetes-views-namespaces)

## Support

For issues or questions, contact the **${{values.team}}** team.
