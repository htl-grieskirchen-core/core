module.exports = {
  "extends": "../../.eslintrc.js",
  "ignorePatterns": [
    "!**/*"
  ],
  "overrides": [
    {
      "files": [
        "*.ts"
      ],
      "parserOptions": {
        "project": [
          "projects/ngx-core-ui/tsconfig.lib.json",
          "projects/ngx-core-ui/tsconfig.spec.json"
        ],
        "createDefaultProgram": true
      },
      "rules": {
        "@angular-eslint/directive-selector": [
          "error",
          {
            "type": "attribute",
            "prefix": "lib",
            "style": "camelCase"
          }
        ],
        "@angular-eslint/component-selector": [
          "error",
          {
            "type": "element",
            "prefix": "lib",
            "style": "kebab-case"
          }
        ]
      }
    },
    {
      "files": [
        "*.html"
      ],
      "rules": {}
    }
  ]
};
