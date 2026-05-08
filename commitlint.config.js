// commitlint.config.js

/** @type {import('@commitlint/types').UserConfig} */
module.exports = {
  extends: ['@commitlint/config-conventional'],

  rules: {
    /*
     * Recommended commit types:
     *
     * feat:     user-visible feature
     * fix:      bug fix
     * perf:     performance improvement
     * refactor: internal code change without behavior change
     * docs:     documentation only
     * test:     tests only
     * build:    build system, dependencies, packaging, MSBuild, NuGet
     * ci:       CI/CD configuration
     * chore:    repository maintenance
     * style:    formatting only, no semantic change
     * revert:   revert a previous commit
     */
    'type-enum': [
      2,
      'always',
      [
        'feat',
        'fix',
        'perf',
        'refactor',
        'docs',
        'test',
        'build',
        'ci',
        'chore',
        'style',
        'revert',
      ],
    ],

    'type-case': [2, 'always', 'lower-case'],
    'type-empty': [2, 'never'],

    /*
     * Scope is optional.
     *
     * Good C#/.NET scopes:
     * core, cli, api, host, di, ef, db, nuget, msbuild,
     * analyzer, source-generator, tests, docs, samples, bench
     *
     * Examples:
     * feat(cli): add parse command
     * fix(ef): correct migration generation
     * build(nuget): include README in package
     * refactor(source-generator): simplify syntax receiver
     */
    'scope-empty': [0],
    'scope-case': [2, 'always', ['lower-case', 'kebab-case', 'pascal-case']],

    /*
     * Subject rules.
     *
     * Disable subject-case because C#/.NET commits often contain names like:
     * IServiceProvider, System.CommandLine, NuGet, MSBuild, EF Core, ASP.NET.
     */
    'subject-empty': [2, 'never'],
    'subject-case': [0],
    'subject-full-stop': [2, 'never', '.'],

    /*
     * Line length.
     *
     * 100 for header is stricter than "anything goes",
     * but not as annoying as 72 in real .NET commits.
     */
    'header-max-length': [2, 'always', 100],

    /*
     * Body/footer formatting.
     * Use warning level for long lines because URLs, stack traces,
     * exception names, and analyzer IDs can be long.
     */
    'body-leading-blank': [1, 'always'],
    'body-max-line-length': [1, 'always', 120],
    'footer-leading-blank': [1, 'always'],
    'footer-max-line-length': [1, 'always', 120],
  },

  /*
   * Keep commitlint's built-in ignores for merge/revert/version commits.
   * Extra WIP ignores are optional; delete them if you want to forbid WIP commits.
   */
  ignores: [
    (message) => message.startsWith('WIP:'),
    (message) => message.startsWith('wip:'),
  ],

  defaultIgnores: true,
};
