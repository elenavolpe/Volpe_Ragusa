package utility

// FindDifferentStrings ritorna due slice di stringhe, la prima contiene le stringhe presenti in slice1 ma non in slice2, la seconda contiene le stringhe presenti in slice2 ma non in slice1

func FindDifferentStrings(slice1, slice2 []string) (s1Diff, s2Diff []string) {
	for _, s1 := range slice1 {
		found := false

		for _, s2 := range slice2 {
			if s1 == s2 {
				found = true
				break
			}
		}

		if !found {
			s1Diff = append(s1Diff, s1)
		}
	}

	for _, s2 := range slice2 {
		found := false

		for _, s1 := range slice1 {
			if s2 == s1 {
				found = true
				break
			}
		}

		if !found {
			s2Diff = append(s2Diff, s2)
		}
	}

	return
}
