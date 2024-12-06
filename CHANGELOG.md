# 1.0.0 (2024-12-06)


### Bug Fixes

* capacity throwing an exception if the pool has been disposed ([ebb5dfb](https://github.com/Hertzole/power-pools/commit/ebb5dfbbd2bba42e1d7285fa93400d9b245eb485))
* fixed size pools not throwing if capacity is negative ([e61f24f](https://github.com/Hertzole/power-pools/commit/e61f24fe8f2ca9571dd2e939993b3e6d86680602))
* FixedSizeObjectPool not throwing if factory is null ([f10bceb](https://github.com/Hertzole/power-pools/commit/f10bcebf24b9c33e45c558ae332f953428934f40))
* negated disposing call on StringBuilderPool ([f54db8f](https://github.com/Hertzole/power-pools/commit/f54db8f1ceed252e8588ae7ca41b0e8f029b2c0f))
* pool.InUse is now 0 if the pool is disposed ([6d103cb](https://github.com/Hertzole/power-pools/commit/6d103cb5618b7febce25309a59476ffe14e1e763))
* StringBuilderPool not using ConfigurablePool directly ([934d6da](https://github.com/Hertzole/power-pools/commit/934d6da3db73cdeb49af55cda6fb8a6aad3c91b6))


### Features

* collection pools ([3cd465a](https://github.com/Hertzole/power-pools/commit/3cd465a8ce0ec31b278d31fb98b24cdf6863b74b))
* FixedSizeObjectPool ([08c14ee](https://github.com/Hertzole/power-pools/commit/08c14ee934cf8a35062e33ed244cc9a13acf0a43))
* Generic(FixedSize)ObjectPool ([6c6eb6d](https://github.com/Hertzole/power-pools/commit/6c6eb6da2eafbc44199d8d478c14b5acdec5752d))
* on dispose callback ([8ce31af](https://github.com/Hertzole/power-pools/commit/8ce31af18da8a50520769fe5041104cc7cb96303))
* support .NET Standard 2.0 ([481a5f2](https://github.com/Hertzole/power-pools/commit/481a5f282cd3ec7b2757b95c7a1452753a8cbe38))
* support for .NET 9 ([df43290](https://github.com/Hertzole/power-pools/commit/df43290da670e7a04e50952b7340c530ba825515))
