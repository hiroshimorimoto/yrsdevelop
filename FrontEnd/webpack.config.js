const path = require('path')

const VueLoaderPlugin = require('vue-loader/lib/plugin')// vue-loader@15から必要
/*const ExtractTextPlugin = require("extract-text-webpack-plugin")*/
const CopyPlugin = require('copy-webpack-plugin');
const webpack = require('webpack');
const Dotenv = require('dotenv-webpack')

console.log('====================================');
console.log("NODE_ENV : " + process.env.NODE_ENV);
console.log("ENV_VAR : " + process.env.ENV_VAR);
console.log('====================================');

module.exports = {
	// configureWebpack: {
	// 	devtool: 'source-map'
	// },
	// エントリポイントのファイル
	mode: process.env.NODE_ENV,
	entry: {
		application: './src/main.application.js',
		manager: './src/main.manager.js',
		provider: './src/main.provider.js',
		customer: './src/main.customer.js'
	},
	output: {
		// 出力ファイル名
		filename: '[name]/bundle.js',
		// 出力先のディレクトリ
		path: path.resolve(__dirname, './dest/')
	},
	devServer: {
		// webpackの扱わないファイル(HTMLや画像など)が入っているディレクトリ
		contentBase: path.resolve(__dirname, 'public'),
		// proxy: {
		// 	"/api/": {
		// 		target: "https://localhost:44387/",
		// 	}
		// }
	},
	module: {
		rules: [
			{
				test: /\.vue$/, // ファイルが.vueで終われば...
				loader: 'vue-loader', // vue-loaderを使う
				options: {
					extractCSS: true,
					loaders: {
						scss: 'vue-style-loader!css-loader!sass-loader', // <style lang="scss">
						sass: 'vue-style-loader!css-loader!sass-loader?indentedSyntax' // <style lang="sass">
					}
				}
			},
			{
				test: /\.js$/,
				loader: 'babel-loader',
			},
			{
				test: /\.css$/,
				use: ['vue-style-loader', 'css-loader'] // css-loader -> vue-style-loaderの順で通していく
			},
			{
				test: /\.(scss)$/,
				use: [{
					loader: 'style-loader', // inject CSS to page
				}, {
					loader: 'css-loader', // translates CSS into CommonJS modules
				}, {
					loader: 'postcss-loader', // Run post css actions
					options: {
						plugins: function () { // post css plugins, can be exported to postcss.config.js
							return [
								require('precss'),
								require('autoprefixer')
							];
						}
					}
				}, {
					loader: 'sass-loader' // compiles Sass to CSS
				}]
			}
			/*,
				loader: 'sass-resources-loader',
				options: {
					resources: path.resolve(__dirname, '/src/style/_custom.scss')
				}
			}
			*/
		]
	},
	resolve: {
		// import './foo.vue' の代わりに import './foo' と書けるようになる(拡張子省略)
		extensions: ['.js', '.vue'],
		alias: {
			// vue-template-compilerに読ませてコンパイルするために必要
			vue$: 'vue/dist/vue.esm.js',
			'@components': path.resolve(__dirname, 'src/components'),
			'@js': path.resolve(__dirname, 'src/js')
		},
	},
	plugins: [
		new VueLoaderPlugin(),
		new webpack.ProvidePlugin({
			$: 'jQuery',
			jQuery: 'jquery',
			'Popper': 'popper.js'
		}),
		new Dotenv({
			path: path.resolve(__dirname, './env/' + process.env.ENV_VAR + '.env'),
			//path: path.join(__dirname, '.env'),
			systemvars: true
		}),
		new CopyPlugin({
			patterns: [
				{ from: './public', to: './' }
			],
		})
	],
	performance: { hints: false } // bundleファイルサイズ警告を非表示
}