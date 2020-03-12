import json

from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import numpy as np

from .preprocessor import preprocess_dataset, get_record_in_text


def get_cosine_similarities(documents):
    vectorizer = TfidfVectorizer(stop_words="english", strip_accents="ascii")
    matrix = vectorizer.fit_transform(documents)
    similarities = cosine_similarity(matrix, matrix)
    return similarities


if __name__ == '__main__':
    groups, _ = preprocess_dataset()

    scores = []
    for group in groups:
        documents = [get_record_in_text(member) for member in group]
        N = len(documents)
        n_pairs = (N * (N - 1)) / 2
        similarities = get_cosine_similarities(documents)
        # print(similarities)
        # print(np.triu(similarities, k=1))
        triu = np.triu(similarities, k=1)
        similarity = np.sum(triu) / n_pairs
        # print(np.sum(triu)/n_pairs)
        scores.append(similarity)

    root = []
    for score, group in zip(scores, groups):
        root.append((score, group))

    root.sort(key=lambda x: x[0], reverse=True)
    root_json_text = json.dumps(root, indent=4)
    print(root_json_text)
    with open("cosine_similarity.json", "w") as f:
        f.write(root_json_text)
