if __name__ == "__main__":
    import unittest
    # Create a test suite
    loader = unittest.TestLoader()
    suite = loader.discover(start_dir='./', pattern='*tests.py')

    # Run the tests
    runner = unittest.TextTestRunner()
    runner.run(suite)
