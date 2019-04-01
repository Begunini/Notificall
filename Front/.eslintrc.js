// https://eslint.org/docs/user-guide/configuring

module.exports = {
  root: true,
  parserOptions: {
    parser: 'babel-eslint'
  },
  env: {
    browser: true,
  },
  extends: [
    // https://github.com/vuejs/eslint-plugin-vue#priority-a-essential-error-prevention
    // consider switching to `plugin:vue/strongly-recommended` or `plugin:vue/recommended` for stricter rules.
    'plugin:vue/essential', 
    // https://github.com/standard/standard/blob/master/docs/RULES-en.md
    'standard'
  ],
  // required to lint *.vue files
  plugins: [
    'vue'
  ],
  globals: {
	  '_': true,
	  '$': true
  },
  // add your custom rules here
  rules: {
	// allow async-await
	'indent': [1, 'tab'],
	'no-tabs': 'off',
	'generator-star-spacing': 'off',
	'spaced-comment': 'off',
	'no-unused-vars': 'warn',
	'no-trailing-spaces': 'off',
	'no-mixed-spaces-and-tabs': 'warn',
	'comma-dangle': 'off', 
	'space-before-function-paren': 'warn',
	'padded-blocks': 'warn',
	'block-spacing': 'warn',
	'space-before-blocks': 'warn',
	'keyword-spacing': 'warn',
	'quotes': 'warn',
	'no-unneeded-ternary': 'off',
	'operator-linebreak': 'off',
	'vue/valid-v-for': 'off',
	'no-multiple-empty-lines': 'warn',
	'eol-last': 'off',
	'comma-spacing': 'warn',
	'camelcase': 'off',
    // allow debugger during development
    'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off'
  }
}
