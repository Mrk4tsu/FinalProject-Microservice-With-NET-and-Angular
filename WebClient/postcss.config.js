module.exports = {
  plugins: {
    autoprefixer: {},
    'postcss-preset-env': {
      stage: 3,
      features: {
        'nesting-rules': false, // Vô hiệu hóa nesting không hợp lệ
      },
    },
  },
};
