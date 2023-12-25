int fib() {
	int n = 10;
	int x = 0;
	int y = 1;	
	int w;

	while(n > 0) {
		w = x + y;
		x = y;
		y = w;
		n -= 1;
	}

	return w;
}
